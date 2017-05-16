using UnityEngine;
using System.Collections;

namespace HG.iot.mqtt
{
	public interface IBrokerConnection
	{
		IIBrokerConnection Notify { get; }
		void AddTopic(ITopic topic);
		void RemoveTopic(ITopic topic);
		string ClientId { get; }
		bool Connect (ConnectionOptions options = null);
		bool Disconnect ();
		bool Reconnect ();
		bool IsConnected { get; }
		string Subscribe(ITopic topic);
		string Unsubscribe(ITopic topic);
		string Send(
			ITopic topic, 
			string message, 
			bool isRetained = false, 
			QualityOfServiceEnum qualityOfService = QualityOfServiceEnum.BestEffort);
	}
}