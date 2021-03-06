﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class Consumption_Receivers : MonoBehaviour
    {
        public string[] id_to_parse;
        public Slider prod_slider;

        public bool debug_mode_on;

        private List<double> prod_list = new List<double>();

        ITopic _cacheGlobalTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            //Debug.Log("onMqttReady invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE",topic.Filter));

            tests(topic);
        }

        // Topic.SimpleNotifications=FALSE
        void onMqttReady_GlobalTopic(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            //Debug.Log("onMqttReady_GlobalTopic invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to FALSE",topic.Filter));
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
                }
            }

            if (debug_mode_on)
            {
                for (int j = 0; j < id_to_parse.Length; j++)
                {

                    _cacheGlobalTopic.Send(
                    //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
                    new GlobalMessage { data = 543.1, t = "2017-05-15T06:47:42Z", id = id_to_parse[j] },
                    false,
                    QualityOfServiceEnum.AtLeastOnce);
                }
            }
        }

        void onMqttMessageDelivered_GlobalTopic(string messageId)
        {
            //Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            //Debug.Log("message just arrived");
            //GlobalMessage receive_obj;

            //Debug.Log("Global value of json object dttp : "+ message.dttp +"data : " + message.data + ", t : " + message.t + ", id : " + message.id);
            
            for (int i = 0; i < id_to_parse.Length; i++)
            {
                prod_list.Add(0.0F);
            }

            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {

                //string json = JsonUtility.ToJson(message);
                //Debug.Log("try to parse json");

                //parse intersting message
                for (int i = 0; i < id_to_parse.Length; i++)
                {
                    //Debug.Log("json message : " + json);
                    //Debug.Log("message that json must contain : " + id_to_parse[i]);
                    if (message.id.Contains(id_to_parse[i]) == true)
                    {
                        //receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                        prod_list[i] = message.data;
                        //Debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                    }
                }
            }

            //else
              //  Debug.LogWarning("message arrived, but failed JSON conversion");
            prod_slider.value = 0.0f;
            float temp_value = 0.0f;
            int j = 0;
            // Debug.Log("Temp value before : " + temp_value);
            for (; j < id_to_parse.Length; j++)
            {
                if (!(prod_list[j] <= 0)) temp_value += (float)prod_list[j]; 
            }
            //Debug.Log("Temp value after : " + temp_value+" ; slide value before : "+prod_slider.value);
            prod_slider.value = temp_value / 1000;
            //Debug.Log("Slide value after : " + prod_slider.value);
        }

        void onMqttSubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription successful");

            /*debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
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
