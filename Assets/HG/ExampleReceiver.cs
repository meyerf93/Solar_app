using UnityEngine;
using System.Collections;
using HG.iot.mqtt;
using System;

namespace HG
{
	/*
	public class ExampleReceiver : MonoBehaviour 
	{
		#region Geolocation Impl

		public LatLong geoMarkerScript;
		private GeolocationMessage _lastTriangulation;

		private class GeolocationMessage
		{
			public float accuracy = 0f;
			public float longitude = 0f;
			public float latitude = 0f;
		}

		private void onTriangulated(string data)
		{
			_lastTriangulation = JsonUtility.FromJson<GeolocationMessage>(data);
			Logger.Log("client.js",string.Format("You have been triangulated at lon={0},lat={1} with {2}% accuracy!",_lastTriangulation.longitude,_lastTriangulation.latitude,_lastTriangulation.accuracy),SEVERITY.WARN);
			geoMarkerScript.PlaceMarker(_lastTriangulation.latitude,_lastTriangulation.longitude);
			triangulated();
		}
			
		#endregion

		#region Unity Readiness Impl

		private class OnUnityReadyMessage
		{
			public string username = string.Empty;
		}

		// sent from javascript
		private void onUnityReady(string data)
		{
			OnUnityReadyMessage message = JsonUtility.FromJson<OnUnityReadyMessage>(data);
			Logger.Log("HGUNITY3DJS","Module sent 'onUnityReady' message. User= " + message.username,SEVERITY.INFO);

			// capture our username
			_lastKnownUsername = message.username;

			// update LWT before connecting
			ChatMessage lwt = new ChatMessage {
				type = "status",
				username = _lastKnownUsername,
				message = "OFFLINE"
			};
			_mqttConnectionOptions.LwtTopic = _chatTopic;
			_mqttConnectionOptions.LwtMessage = JsonUtility.ToJson(lwt);
		}

		#endregion

		#region Chat Implementation

		// default chat topic
		private string _chatTopic = "/unity3d/world";
		private string _lastKnownUsername = string.Empty;

		// structure of chat message
		private class ChatMessage
		{
			public string type = string.Empty;
			public string username = string.Empty;
			public string message = string.Empty;
		}

		// message to be sent.
		// this message arrives from javascript by clicking 'Send Message' button
		private void onChatMessage(string data) 
		{
			ChatMessage message = JsonUtility.FromJson<ChatMessage>(data);

			if(string.IsNullOrEmpty(message.username) || string.IsNullOrEmpty(message.message))
			{
				Logger.Log("client.js","Your message could not be sent with empty values.",SEVERITY.WARN);
				return;
			}

			if(!_mqttManager.IsConnected)
			{
				Logger.Log("client.js","MQTT client is disconnected.  Messages can not be sent at this time.",SEVERITY.WARN);
				return;
			}

			// capture our username change
			if(message.type=="namechange")
				_lastKnownUsername = message.username;

			_mqttManager.Send(new Message { 
				Topic = _chatTopic,
				PayloadString = data,
				QualityOfService = QOS.BEST_EFFORT });
		}

		// message arriving from MQTT broker, could be a chat message, could be something else
		private void listen(Message data)
		{
			ChatMessage message = null;

			// let's try to parse it into a chat message, otherwise skip out
			try {
				message = JsonUtility.FromJson<ChatMessage>(data.PayloadString);
			}
			catch {
				return;
			}

			string format = "<color=orange>{0}: {1}</color>";

			switch (message.type)
			{
				case "conversation":
					Logger.Log("MQTT.incoming",string.Format(format,message.username,message.message),SEVERITY.INFO);
					break;

				case "namechange":
					Logger.Log("MQTT.incoming",string.Format(format,message.username, "Changed Name From '" + message.message + "'"),SEVERITY.INFO);
					break;

				case "status":
					Logger.Log("MQTT.incoming",string.Format(format,message.username, "Status: " + message.message),SEVERITY.INFO);
					break;

				default:
					Logger.Log("MQTT.incoming",string.Format(format,message.username,
					"'" + (string.IsNullOrEmpty(message.type) ? "[null]" : message.type) + "' is an unknown message type."),SEVERITY.INFO);	
					break;
			}
		}

		// let everyone know that you have joined
		private void joined()
		{
			ChatMessage message = new ChatMessage {
				type = "status",
				username = _lastKnownUsername,
				message = "ONLINE"
			};

			_mqttManager.Send(new Message { 
				Topic = _chatTopic,
				PayloadString = JsonUtility.ToJson(message), 
				QualityOfService = QOS.BEST_EFFORT });
		}

		// let everyone know that you have been located on this earth
		private void triangulated()
		{
			ChatMessage message = new ChatMessage {
				type = "status",
				username = _lastKnownUsername,
				message = "TRIANGULATED"
			};

			_mqttManager.Send(new Message { 
				Topic = _chatTopic,
				PayloadString = JsonUtility.ToJson(message), 
				QualityOfService = QOS.BEST_EFFORT });
		}

		#endregion

		[SerializeField] private MQTT _mqttManager = null;
		[SerializeField] private ConnectionOptions _mqttConnectionOptions = null;

		string _topic = "/unity3d/world";

		private bool shouldResubscribe(MqttData data)
		{
			// cleanSession=false does not keep subscription for WS ActiveMQ
			//return !data.autoReconnect || (data.sessionLost || (data.autoReconnect && data.reconnectCount == 0));

			return true;
		}

		private void connected(MqttData data)
		{
			if(shouldResubscribe(data))
			{
				_mqttManager.Subscribe(_topic, QOS.BEST_EFFORT,
					onSuccess: (options) => { subscribed(options); },
					onFailure: (options) => { Logger.Log("MQTT.callback","local callback subscription failure",SEVERITY.WARN); });
			}
		}

		private void subscribed(SubscriptionOptions options)
		{
			Logger.Log("MQTT.receiver","Subscribed to TOPIC= " + options.Topic,SEVERITY.INFO);

			var m = "I have subscribed to '" + _topic + "'. Hello!";

			_mqttManager.Send(new Message { 
				Topic = _topic,
				PayloadString = m, 
				QualityOfService = QOS.BEST_EFFORT },
				onSuccess: (message) => { Logger.Log("MQTT.callback","local callback message send success",SEVERITY.DEBUG); },
				onFailure: (message) => { Logger.Log("MQTT.callback","local callback message send failure",SEVERITY.WARN); });

			joined();
		}

		#region Invocations from JavaScript
		private void MqttReady()
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttReady",SEVERITY.INFO);

			_mqttManager.Connect(_mqttConnectionOptions);
		}

		private void MqttConnected(MqttData data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttConnected",SEVERITY.INFO);

			connected(data);
		}

		private void MqttConnectionFailed(MqttData data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttConnectionFailed",SEVERITY.WARN);
		}

		private void MqttReconnect(MqttData data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttReconnect",SEVERITY.INFO);
		}

		private void MqttConnectionLost(MqttData data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttConnectionLost",SEVERITY.WARN);
		}

		private void MqttMessageDelivered(Message data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttMessageDelivered",SEVERITY.DEBUG);
		}

		private void MqttMessageArrived(Message data)
		{
			Logger.Log("MQTT.receiver",string.Format("Example Receiver: MqttMessageArrived: {0}@{1} ==> {2}", data.Topic, data.QualityOfService.ToString(), data.PayloadString),SEVERITY.VERBOSE);

			listen(data);
		}

		private void MqttSubscriptionSuccess(SubscriptionOptions data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttSubscriptionSuccess",SEVERITY.DEBUG);
		}

		private void MqttSubscriptionFailure(SubscriptionOptions data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttSubscriptionFailure",SEVERITY.WARN);
		}

		private void MqttUnsubscriptionSuccess(SubscriptionOptions data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttUnsubscriptionSuccess",SEVERITY.DEBUG);
		}

		private void MqttUnsubscriptionFailure(SubscriptionOptions data)
		{
			Logger.Log("MQTT.receiver","Example Receiver: MqttUnsubscriptionFailure",SEVERITY.WARN);
		}
		#endregion
	}
	*/
}