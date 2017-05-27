using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
	public class PredictionReceivers: MonoBehaviour 
	{
        public string []id_to_parse;
        public Slider pred_slider;
        public Text text_value;

        private List<float> pred_list = new List<float>();
        
		ITopic _cacheGlobalTopic = null;

		// Topic.SimpleNotifications=TRUE
		void onMqttReady(ITopic topic)
		{
			_cacheGlobalTopic = topic;

			Debug.Log("onMqttReady invoked");
			Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE",topic.Filter));

			//tests(topic);
		}

		// Topic.SimpleNotifications=FALSE
		void onMqttReady_GlobalTopic(ITopic topic)
		{
			_cacheGlobalTopic = topic;

			Debug.Log("onMqttReady_GlobalTopic invoked");
			Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to FALSE",topic.Filter));
			Debug.Log("SimpleNotifications=FALSE give you the flexibility of receiving messages from various topics within the same receiver script");
			Debug.Log("Every notification will be in the format '[notification-method]_[topic-filter]'");

			tests(topic);
		}

		void tests(ITopic topic)
		{
			try
			{
				Debug.Log("Let's try subscribing to this topic without connecting to broker first....");
				topic.Subscribe();
			}
			catch(OperationCanceledException ocex)
			{
				Debug.LogError("Performing actions that require an active connection will throw an 'OperationCancelledException'");
			}

			topic.ConnectionManager.Connect();
		}

		void onMqttMessageDelivered_GlobalTopic(string messageId)
		{
			Debug.Log("message delivered to broker");
		}

        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            ProductionJson test_obj = new ProductionJson();
            test_obj.data = 2.5;
            test_obj.t = "time 19:00:35";
            test_obj.t = "test id";

            for(int i = 0; i < id_to_parse.Length; i++)
            {
                pred_list.Add(0.0F);
            }

            /*Debug.Log("Message arrived on GlobalTopic");
            Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");*/

            if (!message.JSONConversionFailed){ 
                //Debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);
                

                //parse intersting message
                for(int i=0;i<id_to_parse.Length; i++)
                {
                    if (json.Contains(id_to_parse[i]) == true)
                    {
                        test_obj = JsonUtility.FromJson<ProductionJson>(json);
                        pred_list[i] = (float)test_obj.data;
                        //Debug.Log("Value of json object : data : " + test_obj.data + ", t : " + test_obj.t + ", id : " + test_obj.id);
                    }
                }
            }
                
			else
				Debug.LogWarning("message arrived, but failed JSON conversion");

            pred_slider.value = 0.0f;
            for(int i = 0; i < id_to_parse.Length; i++)
            {
                pred_slider.value += pred_list[i];  
            }
            text_value.text= pred_slider.value.ToString();
        }

		void onMqttSubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
		{
			Debug.Log("subscription successful");

			Debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
				" 'Best effort' QOS does not get delivery verification from broker.  " +
				"'Best effort' is however the quickest and dirtiest way to send a message.");
			
			_cacheGlobalTopic.Send(
                //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
                new GlobalMessage { dttp = "this is text", data = "666", t = "2017-05-15T06:47:42Z",  id= "knx1/:1.2.26/:/dim.7" }, 

                false, 
				QualityOfServiceEnum.AtLeastOnce);
		}

		void onMqttSubscriptionFailure_GlobalTopic(SubscriptionResponse response)
		{
			Debug.Log("subscription failed");
		}

		void onMqttUnsubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
		{
			Debug.Log("unsubscription successful");
		}

		void onMqttUnsubscriptionFailure_GlobalTopic(SubscriptionResponse response)
		{
			Debug.Log("unsubscription failed");
		}

		void onMqttConnectSuccess_GlobalTopic(ConnectionResult response)
		{
			Debug.Log("you are connected to broker");
		}

		void onMqttConnectFailure_GlobalTopic(ConnectionResult response)
		{
			Debug.Log("connection to broker failed");
		}

		void onMqttConnectLost_GlobalTopic(ConnectionResult response)
		{
			Debug.Log("connection to broker lost");
		}

		void onMqttReconnect_GlobalTopic(ConnectionResult response)
		{
			Debug.Log("broker has reconnected");
		}
	}
}