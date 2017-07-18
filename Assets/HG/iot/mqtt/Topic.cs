using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HG.iot.mqtt
{
	public abstract class Topic<TMessage> : MonoBehaviour, ITopic, IITopic
		where TMessage : Message
	{
        private int debugCounter = 0;
        private ITopic topic {
			get {
				return this;
			}
		}

		void Awake()
		{
            Debug.Log("debugger counter : " + ++debugCounter);
			Statistics = new TopicStats();

			_connection = BrokerConnection.Instance;

			if(_connection==null)
			{
				Debug.LogError("Unable to find component implementing IMqttConnection interface!");
				return;
			}
			else
			{
				Debug.Log("Found component implementing IMqttConnection interface.");
				_connection.AddTopic(this);
			}
		}

		private IBrokerConnection _connection;

		private Type topicType { 
			get {
				return this.GetType();
			}
		}

		private Type messageType {
			get {
				return typeof(TMessage);
			}
		}

		[SerializeField]
		protected bool SimpleNotifications = false;

		[SerializeField]
		protected List<GameObject> Receivers = new List<GameObject>();

		protected virtual void notifyReceivers(string methodName, object obj)
		{
			foreach(var go in Receivers)
			{
				if(SimpleNotifications)
					MainThreadInvoke.Instance.Add(() => {
						go.SendMessage(methodName,obj,SendMessageOptions.DontRequireReceiver);
					});
				else
					MainThreadInvoke.Instance.Add(() => {
						go.SendMessage(methodName+"_"+topicType.Name,obj,SendMessageOptions.DontRequireReceiver);
					});
			}
		}

		protected struct QueuedMessage
		{
			public Message Message;
			public bool IsRetained;
			public QualityOfServiceEnum QualityOfService;
		}

		protected Queue<QueuedMessage> outboundQueue = new Queue<QueuedMessage>();

		protected virtual void processOutboundQueue()
		{
			while(outboundQueue.Count>0)
			{
				var qm = outboundQueue.Dequeue();
				_connection.Send(this, JsonUtility.ToJson(qm.Message), qm.IsRetained, qm.QualityOfService);
			}
		}

		#region ITopic

		public TopicStats Statistics { get; private set; }

		public IBrokerConnection ConnectionManager {
			get {
				return _connection;
			}
		}

		public IITopic Notify {
			get {
				return (IITopic)this;
			}
		}

		public MonoBehaviour Script {
			get {
				return this;
			}
		}

		public Type TopicType { 
			get {
				return this.GetType();
			}
		}

		public Type MessageType { 
			get {
				return typeof(TMessage);
			}
		}

		[SerializeField]
		protected int subscriptionTimeout = 15;

		public int SubscriptionTimeout {
			get {
				return subscriptionTimeout;
			}
		}

		[SerializeField]
		protected string filter = string.Empty;

		public string Filter {
			get {
				return filter;
			}
		}

		[SerializeField]
		protected bool subscribeOnConnect = true;

		public bool SubscribeOnConnect {
			get {
				return subscribeOnConnect;
			}
		}

		[SerializeField]
		protected bool allowEmptyMessage = false;

		public bool AllowEmptyMessage {
			get {
				return allowEmptyMessage;
			}
		}

		protected bool isSubscribed = false;

		public virtual bool IsSubscribed {
			get {
				return isSubscribed;
			}
			private set {
				isSubscribed = value;
			}
		}
			
		[SerializeField]
		protected QualityOfServiceEnum requestedQualityOfService = QualityOfServiceEnum.BestEffort;

		public QualityOfServiceEnum RequestedQualityOfService {
			get {
				return requestedQualityOfService;
			}
		}

		private QualityOfServiceEnum _grantedQualityOfService = QualityOfServiceEnum.BestEffort;

		public QualityOfServiceEnum GrantedQualityOfService {
			get {
				return _grantedQualityOfService;
			}
		}

		public virtual void Subscribe(QualityOfServiceEnum qualityOfService = QualityOfServiceEnum.Undefined) 
		{
			if(!_connection.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");

			if(qualityOfService != QualityOfServiceEnum.Undefined)
				requestedQualityOfService = qualityOfService;

			if(requestedQualityOfService == QualityOfServiceEnum.Undefined)
				requestedQualityOfService = QualityOfServiceEnum.BestEffort;

			_connection.Subscribe(this);
		}

		public virtual string Send(Message message, bool isRetained = false, QualityOfServiceEnum qualityOfService = QualityOfServiceEnum.BestEffort)
		{
			if(!_connection.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");
			
			Statistics.MessagesQueued += 1;

			if(!_connection.IsConnected)
			{
				outboundQueue.Enqueue(new QueuedMessage { Message = message, QualityOfService = qualityOfService });
				Debug.LogWarning("Connection to broker is not available. Your message has been queued for later delivery.");
				return string.Empty;
			}

			string payload = string.Empty;

			try
			{
				payload = JsonUtility.ToJson(message);
			}
			catch(ArgumentException aex)
			{
				Debug.LogErrorFormat(aex.ToString()+"'{0}' message failed serialization from string '{1}'", this.MessageType.Name, message);
			}

			if(string.IsNullOrEmpty(payload) && !allowEmptyMessage)
			{
				Debug.LogErrorFormat("'{0}' topic does not allow empty messages!", this.TopicType.Name);
				return string.Empty;
			}

			return _connection.Send(this, payload, isRetained, qualityOfService);
		}

		public virtual void Unsubscribe() 
		{
			if(!_connection.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");

			_connection.Unsubscribe(this);
		}

		#endregion

		#region IITopic

		public virtual void onMqttReady() 
		{
			notifyReceivers("onMqttReady",this.topic);
		}

		public virtual void onMqttMessageDelivered(string id) 
		{
			topic.Statistics.MessagesSent += 1;

			notifyReceivers("onMqttMessageDelivered",id);
		}

		public virtual void onMqttMessageNotDelivered(string id) 
		{
			topic.Statistics.MessagesNotSent += 1;

			notifyReceivers("onMqttMessageNotDelivered",id);
		}

		public virtual void onMqttMessageArrived(string message)
		{
			if(!isSubscribed)
			{
				topic.Statistics.MessagesDropped += 1;
				Debug.LogWarning(string.Format("Message arrived on topic '{0}' but was dropped because the topic is not subscribed.", topicType.Name));
				return;
			}

			topic.Statistics.MessagesReceived += 1;

			TMessage msg = null;

			try
			{
				msg = JsonUtility.FromJson<TMessage>(message);

				if(msg==null)
				{
					msg = Activator.CreateInstance<TMessage>();
					msg.JSONConversionFailed = true;
					msg.ArrivedEmpty = true;
				}
			}
			catch(ArgumentException aex)
			{
				if(!allowEmptyMessage)
					Debug.LogErrorFormat(aex.ToString()+"'{0}' message failed deserialization from string '{1}'", this.MessageType.Name, message);

				msg = Activator.CreateInstance<TMessage>();
				msg.JSONConversionFailed = true;
			}

			msg.OriginalMessage = message;

			notifyReceivers("onMqttMessageArrived",msg);
		}

		public virtual void onMqttSubscriptionSuccess(SubscriptionResponse response) 
		{
			_grantedQualityOfService = response.GrantedQualityOfService;

			isSubscribed = true;

			notifyReceivers("onMqttSubscriptionSuccess", response);
		}

		public virtual void onMqttSubscriptionFailure(SubscriptionResponse response) 
		{
			notifyReceivers("onMqttSubscriptionFailure", response);
		}
			
		public virtual void onMqttUnsubscriptionSuccess(SubscriptionResponse response) 
		{
			isSubscribed = false;

			notifyReceivers("onMqttUnsubscriptionSuccess", response);
		}

		public virtual void onMqttUnsubscriptionFailure(SubscriptionResponse response) 
		{
			notifyReceivers("onMqttUnsubscriptionFailure", response);
		}

		public virtual void onMqttConnectSuccess(ConnectionResult result)
		{
			if(subscribeOnConnect)
				topic.Subscribe();

			processOutboundQueue();

			notifyReceivers("onMqttConnectSuccess", result);
		}

		public virtual void onMqttConnectFailure(ConnectionResult result)
		{
			notifyReceivers("onMqttConnectFailure", result);
		}

		public virtual void onMqttConnectLost(ConnectionResult result)
		{
			notifyReceivers("onMqttConnectLost", result);
		}

		public virtual void onMqttReconnect(ConnectionResult result)
		{
			notifyReceivers("onMqttReconnect", result);
		}

		#endregion
	}
}