using UnityEngine;
using System.Collections;

namespace HG.iot.mqtt
{
	public class ConnectionResult
	{
		public string ContextId = string.Empty;
		public string ClientId = string.Empty;
		public int ErrorCode = 0;
		public string ErrorMessage = string.Empty;
	}
}
