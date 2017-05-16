using UnityEngine;
using System.Collections;
using System;

namespace HG.iot.mqtt
{
	public class SubscriptionResponse
	{
		public string Id;
		public QualityOfServiceEnum GrantedQualityOfService = QualityOfServiceEnum.Undefined;
		public int ErrorCode = 0;
		public string ErrorMessage = string.Empty;
	}
}