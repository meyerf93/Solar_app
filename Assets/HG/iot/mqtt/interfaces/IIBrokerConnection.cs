﻿using UnityEngine;
using System.Collections;

namespace HG.iot.mqtt
{
	public interface IIBrokerConnection
	{
		void onMessageArrived(InboundMessage message);
		void onConnectSuccess(ConnectionResult result);
		void onConnectFailure(ConnectionResult result);
		void onConnectLost(ConnectionResult result);
		void onReconnect(ConnectionResult result);
	}
}