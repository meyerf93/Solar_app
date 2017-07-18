using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HG.iot.mqtt
{
	public class BrokerConnection : Singleton<BrokerConnection>, IBrokerConnection, IIBrokerConnection
	{
		protected BrokerConnection() { }

		void Awake()
		{
			_client = gameObject.GetComponent(typeof(IMqttClient)) as IMqttClient;

			if(_client==null)
			{
				Debug.LogError("Unable to find component implementing IMqttClient interface!");
				return;
			}
			else
			{
				Debug.Log("Found component implementing IMqttClient interface.");

				try
				{
					_client.Init(this);
				}
				catch (PlatformNotSupportedException pnsex)
				{
					Debug.LogError(pnsex.ToString()+"Component implementing IMqttClient interface does not support the current platform.");
				}
			}
		}

		void Start()
		{
			foreach(var t in _topics)
				t.Notify.onMqttReady();
		}

		public IIBrokerConnection Notify {
			get {
				return (IIBrokerConnection)this;
			}
		}

		#region known topics

		private List<ITopic> _topics = new List<ITopic>();

		public void AddTopic(ITopic topic)
		{
			_topics.Add(topic);
		}

		public void RemoveTopic(ITopic topic)
		{
			_topics.Remove(topic);
		}

		#endregion

		#region client interaction

		private IMqttClient _client = null;

		private string _clientId = string.Empty;

		public string ClientId {
			get {
				return _clientId;
			}
		}

		[SerializeField]
		private ConnectionOptions _defaultConnectionOptions = new ConnectionOptions();

		public bool Connect (ConnectionOptions options = null)
		{
			return _client.Connect(options==null ? _defaultConnectionOptions : options);
		}

		public bool Disconnect ()
		{
			if(!this.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");

			return _client.Disconnect();
		}

		public bool Reconnect ()
		{
			return _client.Reconnect();
		}
			
		public string Send(ITopic topic, string message, bool isRetained = false, QualityOfServiceEnum qualityOfService = QualityOfServiceEnum.BestEffort)
		{
			if(!this.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");

			var wrappedMessage = new OutboundMessage { 
				Topic = topic,
				Message = message,
				IsRetained = isRetained,
				QualityOfService = qualityOfService,
				OnSuccess = (m) => { topic.Notify.onMqttMessageDelivered(m.Id); },
				OnFailure = (m) => { topic.Notify.onMqttMessageNotDelivered(m.Id); }
			};

			OutboundMessage wm = _client.Send(wrappedMessage);

			return wm.Id;
		}

		private bool _isConnected = false;

		public bool IsConnected {
			get {
				return _isConnected;
			}
		}
			
		public string Subscribe(ITopic topic)
		{
			if(!this.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");

			var wrappedSub = new WrappedSubscription {
				Topic = topic,
				OnSuccess = (s) => { 
					topic.Notify.onMqttSubscriptionSuccess( 
						new SubscriptionResponse {
							Id = s.Id,
							GrantedQualityOfService = s.GrantedQualityOfService
						}); 
				},
				OnFailure = (s) => { 
					topic.Notify.onMqttSubscriptionFailure( 
						new SubscriptionResponse {
							Id = s.Id,
							ErrorCode = s.ErrorCode,
							ErrorMessage = s.ErrorMessage
						}); 
				}
			};

			WrappedSubscription ws = _client.Subscribe(wrappedSub);

			return ws.Id;
		}

		public string Unsubscribe(ITopic topic)
		{
			if(!this.IsConnected)
				throw new OperationCanceledException("Connection object is not in a valid state.");

			var wrappedSub = new WrappedSubscription {
				Topic = topic,
				OnSuccess = (s) => { 
					topic.Notify.onMqttUnsubscriptionSuccess(
						new SubscriptionResponse {
							Id = s.Id
						}); 
				},
				OnFailure = (s) => { 
					topic.Notify.onMqttUnsubscriptionFailure(
						new SubscriptionResponse {
							Id = s.Id,
							ErrorCode = s.ErrorCode,
							ErrorMessage = s.ErrorMessage
						}); 
				}
			};

			WrappedSubscription ws = _client.Unsubscribe(wrappedSub);

			return ws.Id;
		}

		// dispatch inbound message to correct topics
		public void onMessageArrived(InboundMessage message)
		{
			//aTODO: ADD WILDCARD SUPPORT

			foreach(var topic in _topics.Where(t => t.Filter.Equals(message.Topic)))
				topic.Notify.onMqttMessageArrived(message.Message);
		}

		#endregion

		#region pass connection related notifications from client to all topics

		public void onConnectSuccess(ConnectionResult result)
		{
			_clientId = result.ClientId;
			_isConnected = true;

			foreach(var t in _topics)
				t.Notify.onMqttConnectSuccess(result);
		}

		public void onConnectFailure(ConnectionResult result)
		{
			_isConnected = false;

			foreach(var t in _topics)
				t.Notify.onMqttConnectFailure(result);
		}

		public void onConnectLost(ConnectionResult result)
		{
			_isConnected = false;

			foreach(var t in _topics)
				t.Notify.onMqttConnectLost(result);
		}

		public void onReconnect(ConnectionResult result)
		{
			_isConnected = true;

			foreach(var t in _topics)
				t.Notify.onMqttReconnect(result);
		}

		#endregion
	}
}