using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HG.iot.mqtt.example
{
    public class LightSenders : MonoBehaviour
    {
        public string id_to_parse;
        public string mdl;
        public bool debug_mode_on;

        public void OnChange(float value)
        {
            _cacheSetValueTopic.Send(
            //{"cmd": "knx1/:1.1.26/:/dim.1","mdl"="knx1","value": 100.0}
            new SetValueMessage { cmd = id_to_parse, mdl = mdl, value = value },
            false,
            QualityOfServiceEnum.AtLeastOnce);
        }

        ITopic _cacheSetValueTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheSetValueTopic = topic;

            //Debug.Log("onMqttReady invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE", topic.Filter));

            tests(topic);
        }
        // Topic.SimpleNotifications=FALSE

        void onMqttReady_SetValueTopic(ITopic topic)
        {
            _cacheSetValueTopic = topic;

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
                _cacheSetValueTopic.Send(
                //{"cmd": "knx1/:1.1.26/:/dim.1","mdl"="knx1","value": 100.0}}
                new SetValueMessage { cmd = "knx1/:1.1.26/:/dim.1", mdl = "knx1", value = 100.0 },
                false,
                QualityOfServiceEnum.ExactlyOnce);
            }
        }

        void onMqttMessageDelivered_SetValueTopic(string messageId)
        {
            //Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_SetValueTopic(SetValueMessage message)
        {
            //Debug.Log("message just arrived");
        
            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");
        }

        void onMqttSubscriptionSuccess_SetValueTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription successful");

            /*Debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
                " 'Best effort' QOS does not get delivery verification from broker.  " +
                "'Best effort' is however the quickest and dirtiest way to send a message.");*/

        }

        void onMqttSubscriptionFailure_SetValueTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription failed");
        }

        void onMqttUnsubscriptionSuccess_SetValueTopic(SubscriptionResponse response)
        {
            //Debug.Log("unsubscription successful");
        }

        void onMqttUnsubscriptionFailure_SetValueTopic(SubscriptionResponse response)
        {
            //Debug.Log("unsubscription failed");
        }

        void onMqttConnectSuccess_SetValueTopic(ConnectionResult response)
        {
            //Debug.Log("you are connected to broker");
        }

        void onMqttConnectFailure_SetValueTopic(ConnectionResult response)
        {
            //Debug.Log("connection to broker failed");
        }

        void onMqttConnectLost_SetValueTopic(ConnectionResult response)
        {
            //Debug.Log("connection to broker lost");
        }

        void onMqttReconnect_SetValueTopic(ConnectionResult response)
        {
            //Debug.Log("broker has reconnected");
        }
    }
}
