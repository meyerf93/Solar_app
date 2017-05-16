using UnityEngine;
using System.Collections;
using System;

namespace HG.iot.mqtt
{
	[Serializable]
	public class ConnectionOptions
	{
		#if UNITY_WEBGL && !UNITY_EDITOR
		public int Port = 61614;
		#else
		public int Port = 1883;
		#endif

		public string Host = "localhost";

		public bool RandomClientId = true;

		public string Path = "/mqtt";

		public bool AutoReconnect = true;

		public int ReconnectDelayMs = 5000;

		public int Timeout = 30;

		public string Username = string.Empty;

		public string Password = string.Empty;

		public int KeepAliveInterval = 60;

		public bool CleanSession = true;

		public bool UseSSL = false;

		public string InvocationContext = System.Guid.NewGuid().ToString();

		public string[] Hosts;

		public int[] Ports;

		public MQTTVersionEnum ProtocolVersion = MQTTVersionEnum.MQTT_3_1_1;

		public string LwtTopic = "unity/iot/lwt";

		public string LwtMessage = "good_bye";

		public QualityOfServiceEnum LwtQos = QualityOfServiceEnum.BestEffort;

		public string ClientId {
			get 
			{
				if(RandomClientId) {
					if(string.IsNullOrEmpty(_clientId))
						_clientId = System.Guid.NewGuid().ToString();
				}
				else {
					_clientId = "UNITY3D-MQTT-CLIENT";
				}

				return _clientId;
			}

			set 
			{
				_clientId = value;
			}
		}

		private string _clientId = string.Empty;
	}
}