using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HG.iot.mqtt
{
	[Serializable]
	public abstract class Message
	{
		[NonSerialized]
		public bool JSONConversionFailed = false;

		[NonSerialized]
		public bool ArrivedEmpty = false;

		[NonSerialized]
		public string OriginalMessage = string.Empty;

		public Message()
		{
			setTimestamp();
		}

		private void setTimestamp()
		{
			if(timestamp!=0)
				return;

			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			timestamp = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);
		}

		public long timestamp = 0;
	}
}