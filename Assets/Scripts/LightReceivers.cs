using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class LightReceivers : MonoBehaviour
    {
        public string id_to_parse;
        public Slider slider_value;
        public void OnChange(float value)
        {
            _cacheGlobalTopic.Send(
            //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
            new GlobalMessage { dttp = "", data = value, t = DateTime.Today.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"), id = id_to_parse },
                false,
                QualityOfServiceEnum.AtLeastOnce);
        }

        ITopic _cacheGlobalTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            Debug.Log("onMqttReady invoked");
            Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE", topic.Filter));

            tests(topic);
        }

        // Topic.SimpleNotifications=FALSE
        void onMqttReady_GlobalTopic(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            Debug.Log("onMqttReady_GlobalTopic invoked");
            Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to FALSE", topic.Filter));
            Debug.Log("SimpleNotifications=FALSE give you the flexibility of receiving messages from various topics within the same receiver script");
            Debug.Log("Every notification will be in the format '[notification-method]_[topic-filter]'");

            tests(topic);
        }

        void tests(ITopic topic)
        {
            if (!topic.ConnectionManager.IsConnected)
            {
                topic.ConnectionManager.Connect();
            }
            if (!topic.IsSubscribed)
            {
                try
                {
                    Debug.Log("Let's try subscribing to this topic without connecting to broker first....");
                    topic.Subscribe();
                }
                catch (OperationCanceledException ocex)
                {
                    Debug.LogError(ocex.ToString() + "Performing actions that require an active connection will throw an 'OperationCancelledException'");
                    Debug.Log("bonjour monsieur MQTT !");
                }
            }


            _cacheGlobalTopic.Send(
            //{"dttp": null, "data": 22.5, "t": "2017-05-15T06:47:42Z", "id": "zwave1/:3260679919/:2/:/infos.1/:/1/:/1"}
            new GlobalMessage { dttp = "this is text", data = 45, t = "2017-05-15T06:47:42Z", id = id_to_parse },

            false,
            QualityOfServiceEnum.ExactlyOnce);



        }

        void onMqttMessageDelivered_GlobalTopic(string messageId)
        {
            Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            Debug.Log("message just arrived");
            GlobalMessage receive_obj;

            Debug.Log("Message arrived on GlobalTopic");
            Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {
                Debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);
                Debug.Log(id_to_parse);
                //parse intersting message
                if (json.Contains(id_to_parse))
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    slider_value.value = (float)receive_obj.data;
                    Debug.Log("Value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                }
            }

            else
                Debug.LogWarning("message arrived, but failed JSON conversion");
        }

  

        void onMqttSubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            Debug.Log("subscription successful");

            Debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
                " 'Best effort' QOS does not get delivery verification from broker.  " +
                "'Best effort' is however the quickest and dirtiest way to send a message.");

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