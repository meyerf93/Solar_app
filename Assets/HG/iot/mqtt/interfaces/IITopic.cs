using UnityEngine;
using System.Collections;
using System;

namespace HG.iot.mqtt
{
	public interface IITopic
	{
		void onMqttReady();
		void onMqttMessageDelivered(string id);
		void onMqttMessageNotDelivered(string id);
		void onMqttMessageArrived(string message);
		void onMqttSubscriptionSuccess(SubscriptionResponse response);
		void onMqttSubscriptionFailure(SubscriptionResponse response);
		void onMqttUnsubscriptionSuccess(SubscriptionResponse response);
		void onMqttUnsubscriptionFailure(SubscriptionResponse response);
		void onMqttConnectSuccess(ConnectionResult result);
		void onMqttConnectFailure(ConnectionResult result);
		void onMqttConnectLost(ConnectionResult result);
		void onMqttReconnect(ConnectionResult result);
	}
}