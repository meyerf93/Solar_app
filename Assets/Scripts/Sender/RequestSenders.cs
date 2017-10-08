using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HG.iot.mqtt.example
{
    public class RequestSenders : MonoBehaviour
    {
        public String[] id_parse_list;

        ITopic _cacheRequestTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheRequestTopic = topic;

            //Debug.Log("onMqttReady invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE", topic.Filter));

            tests(topic);
        }

        // Topic.SimpleNotifications=FALSE
        void onMqttReady_RequestTopic(ITopic topic)
        {
            _cacheRequestTopic = topic;

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

            for (int i= 0; i< id_parse_list.Length; i++)
            {
                //print("value of id :" + id_parse_list[i] + ", value of mdl : " + id_parse_list[i].Split('/')[0]);
                _cacheRequestTopic.Send(
                new RequestMessage { cmd = id_parse_list[i], mdl = id_parse_list[i].Split('/')[0] },
                false,
                QualityOfServiceEnum.ExactlyOnce);
            }
        }

        void onMqttMessageDelivered_RequestTopic(string messageId)
        {
            //Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_RequestTopic(RequestMessage message)
        {
            //Debug.Log("message just arrived");

            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");
        }

        void onMqttSubscriptionSuccess_RequestTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription successful");

            /*Debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
                " 'Best effort' QOS does not get delivery verification from broker.  " +
                "'Best effort' is however the quickest and dirtiest way to send a message.");*/

        }

        void onMqttSubscriptionFailure_RequestTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription failed");
        }

        void onMqttUnsubscriptionSuccess_RequestTopic(SubscriptionResponse response)
        {
            //Debug.Log("unsubscription successful");
        }

        void onMqttUnsubscriptionFailure_RequestTopic(SubscriptionResponse response)
        {
            //Debug.Log("unsubscription failed");
        }

        void onMqttConnectSuccess_RequestTopic(ConnectionResult response)
        {
            //Debug.Log("you are connected to broker");
        }

        void onMqttConnectFailure_RequestTopic(ConnectionResult response)
        {
            //Debug.Log("connection to broker failed");
        }

        void onMqttConnectLost_RequestTopic(ConnectionResult response)
        {
            //Debug.Log("connection to broker lost");
        }

        void onMqttReconnect_RequestTopic(ConnectionResult response)
        {
            //Debug.Log("broker has reconnected");
        }
    }
}

