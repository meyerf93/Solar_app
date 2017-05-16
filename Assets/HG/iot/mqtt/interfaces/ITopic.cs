using UnityEngine;
using System.Collections;
using System;

namespace HG.iot.mqtt
{
	public interface ITopic
	{
		IBrokerConnection ConnectionManager { get; }
		Type TopicType { get; }
		Type MessageType { get; }
		IITopic Notify { get; }
		MonoBehaviour Script { get; }
		int SubscriptionTimeout { get; }
		string Filter { get; }
		bool SubscribeOnConnect { get; }
		bool IsSubscribed { get; }
		bool AllowEmptyMessage { get; }
		QualityOfServiceEnum RequestedQualityOfService { get; }
		QualityOfServiceEnum GrantedQualityOfService { get; }
		TopicStats Statistics { get; }
		void Subscribe(QualityOfServiceEnum qualityOfService = QualityOfServiceEnum.Undefined);
		string Send(Message message, bool isRetained = false, QualityOfServiceEnum qualityOfService = QualityOfServiceEnum.BestEffort);
		void Unsubscribe();
	}
}