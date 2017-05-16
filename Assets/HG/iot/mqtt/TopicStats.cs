using UnityEngine;
using System.Collections;

namespace HG.iot.mqtt
{
	public class TopicStats
	{
		public int MessagesQueued=0;
		public int MessagesSent=0;
		public int MessagesNotSent=0;
		public int MessagesReceived=0;
		public int MessagesDropped=0;
	}
}
