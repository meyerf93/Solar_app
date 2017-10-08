using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HG.iot.mqtt
{
	public abstract class Topic<TMessage> : MonoBehaviour, ITopic, IITopic
		where TMessage : Message
	{
        //private int debugCounter = 0;
        private ITopic topic {
			get {
				return this;
			}
		}

		void Awake()
		{
            //Debug.Log("debugger counter : " + ++debugCounter);
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
                Debug.Log("name of the topic added : " + this.Filter);
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
            //Debug.Log("number of receivers for  topic :  ; " + Receivers.Count);
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
            //print("message arrive : " + message);
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
                //[{"dttp": {"uts": "W", "tag": "pw", "id": 1, "name": "Power"}, "data": 681.5899658203125, "t": "2017-10-04T15:04:55Z", "id": "knx1/:1.1.19/:/power.1"}]
                //print("mqtt message receive : " + message+","+ String.IsNullOrEmpty(message)+","+message.Contains("null"));

                //print("pass the null test");
                //print("test null, " + !message.Contains("null"));
                if (message.Contains("[") || message.Contains("]") && !message.Contains("null"))
                {
                    if (message.Contains("zwave1"))
                    {
                                                // message
                        //[{"dttp": null, "data": 24.8, "t": "2017-10-07T20:45:14Z", "id": "zwave1/:3260679919/:3/:/infos.1/:/1/:/1"}, 
                        //{ "dttp": null, "data": 89, "t": "2017-10-07T20:45:16Z", "id": "zwave1/:3260679919/:3/:/infos.1/:/1/:/3"},
                        //{ "dttp": null, "data": 0, "t": "2017-10-07T20:45:16Z", "id": "zwave1/:3260679919/:3/:/infos.1/:/1/:/27"}, 
                        //{ "dttp": null, "data": 24, "t": "2017-10-07T20:45:15Z", "id": "zwave1/:3260679919/:3/:/infos.1/:/1/:/5"}]

                        if (message.Contains("[") || message.Contains("]"))
                        {
                            message = message.Replace("[", "").Replace("]", "");
                            message = message.Replace("{","").Replace("}","");
                        }

                        string[] temp_string_array = message.Split(new string[] { "," },StringSplitOptions.None);
                        //print("message lengt number of split : "+temp_string_array.Length);


                        for (int i = 0 ; i < temp_string_array.Length; i++)
                        {
                            if(i%4 == 0)
                            {
                                message = "{" + temp_string_array[i] + "," +
                                              temp_string_array[i + 1] + "," +
                                              temp_string_array[i + 2] + "," +
                                              temp_string_array[i + 3] + "}";
                                msg = JsonUtility.FromJson<TMessage>(message);
                                if (msg == null)
                                {
                                    print("empty message : " + message);
                                    msg = Activator.CreateInstance<TMessage>();
                                    msg.JSONConversionFailed = true;
                                    msg.ArrivedEmpty = true;
                                }

                                msg.OriginalMessage = message;
                                notifyReceivers("onMqttMessageArrived", msg);
                            }
                            //print("message after filtring : " + temp_string_array[i]);                       
                        }
                        //print("message after recomposition : " + message);
                    }
                    else if(!message.Contains("[]"))
                    {
                        if (!message.Contains("xcom1"))
                        {
                            message = message.Replace("[", "").Replace("]", "");
                        }

                        msg = JsonUtility.FromJson<TMessage>(message);
                        if (msg == null)
                        {
                            print("empty message : " + message);
                            msg = Activator.CreateInstance<TMessage>();
                            msg.JSONConversionFailed = true;
                            msg.ArrivedEmpty = true;
                        }

                        msg.OriginalMessage = message;
                        notifyReceivers("onMqttMessageArrived", msg);
                    }

                }
                else if(!message.Contains("null"))
                {
                    //print("message : " + message);
                    msg = JsonUtility.FromJson<TMessage>(message);
                    if (msg == null)
                    {
                        print("empty message : " + message);
                        msg = Activator.CreateInstance<TMessage>();
                        msg.JSONConversionFailed = true;
                        msg.ArrivedEmpty = true;
                    }

                    msg.OriginalMessage = message;
                    notifyReceivers("onMqttMessageArrived", msg);
                }
                else if(message.Contains("null"))
                {
                    //print("message with null arrive : " + message);

                    message = message.Replace("null", "\"\"");

                    if (!message.Contains("xcom1"))
                    {
                        message = message.Replace("[", "").Replace("]", "");
                    }

                    msg = JsonUtility.FromJson<TMessage>(message);
                    if (msg == null)
                    {
                        print("empty message : " + message);
                        msg = Activator.CreateInstance<TMessage>();
                        msg.JSONConversionFailed = true;
                        msg.ArrivedEmpty = true;
                    }

                    msg.OriginalMessage = message;
                    notifyReceivers("onMqttMessageArrived", msg);

                }
            }
			catch(ArgumentException aex)
			{
				if(!allowEmptyMessage)
					Debug.LogErrorFormat(aex.ToString()+"'{0}' message failed deserialization from string '{1}'", this.MessageType.Name, message);

				msg = Activator.CreateInstance<TMessage>();
				msg.JSONConversionFailed = true;
			}
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