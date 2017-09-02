using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HG.iot.mqtt.example
{

    public class AutomaticModeSender : MonoBehaviour
    {
        public string id_to_parse;
        public bool debug_mode_on;
 
        public void OnChange(float value)
        {
            _cacheGlobalTopic.Send(
            //{"dttp":"","data":1.0,"t":2017-05-15T06:47:42Z","id":"car1/:1/automatic.1"}
            new GlobalMessage { dttp = "", data = value, t = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), id = id_to_parse },
                false,
                QualityOfServiceEnum.AtLeastOnce);
        }

        ITopic _cacheGlobalTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            //Debug.Log("onMqttReady invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE", topic.Filter));

            tests(topic);
        }
        // Topic.SimpleNotifications=FALSE

        void onMqttReady_GlobalTopic(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            //Debug.Log("onMqttReady_GlobalTopic invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to FALSE", topic.Filter));
            //Debug.Log("SimpleNotifications=FALSE give you the flexibility of receiving messages from various topics within the same receiver script");
            //Debug.Log("Every notification will be in the format '[notification-method]_[topic-filter]'");

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
                    //Debug.Log("Let's try subscribing to this topic without connecting to broker first....");
                    topic.Subscribe();
                }
                catch (OperationCanceledException ocex)
                {
                    Debug.LogError(ocex.ToString() + "Performing actions that require an active connection will throw an 'OperationCancelledException'");
                    //Debug.Log("bonjour monsieur MQTT !");
                }
            }

            if (debug_mode_on)
            {
                _cacheGlobalTopic.Send(
                //{"dttp":"","data":1.0,"t":2017-05-15T06:47:42Z","id":"car1/:1/automatic.1"}
                new GlobalMessage { dttp = "", data = 1.0, t = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), id = id_to_parse },
                    false,
                    QualityOfServiceEnum.AtLeastOnce);
            }
        }

        void onMqttMessageDelivered_GlobalTopicc(string messageId)
        {
            //Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            //Debug.Log("message just arrived");

            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");
        }

        void onMqttSubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription successful");

            /*Debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
                " 'Best effort' QOS does not get delivery verification from broker.  " +
                "'Best effort' is however the quickest and dirtiest way to send a message.");*/

        }

        void onMqttSubscriptionFailure_GlobalTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription failed");
        }

        void onMqttUnsubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            //Debug.Log("unsubscription successful");
        }

        void onMqttUnsubscriptionFailure_GlobalTopic(SubscriptionResponse response)
        {
            //Debug.Log("unsubscription failed");
        }

        void onMqttConnectSuccess_GlobalTopic(ConnectionResult response)
        {
            //Debug.Log("you are connected to broker");
        }

        void onMqttConnectFailure_GlobalTopic(ConnectionResult response)
        {
            //Debug.Log("connection to broker failed");
        }

        void onMqttConnectLost_GlobalTopic(ConnectionResult response)
        {
            //Debug.Log("connection to broker lost");
        }

        void onMqttReconnect_GlobalTopic(ConnectionResult response)
        {
            //Debug.Log("broker has reconnected");
        }
    }

}
