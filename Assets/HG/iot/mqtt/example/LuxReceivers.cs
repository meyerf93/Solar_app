using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class LuxReceivers : MonoBehaviour
    {
        public string[] id_to_parse;
        public double[] lux_list_activity;

        public Queing_notification queuing;
        private Notifications send_notif;

        private bool mesage_receive = false;

        //public Text content;

        private List<double> temp_list = new List<double>();

        ITopic _cacheGlobalTopic = null;
        public List<Double> getLuxValue()
        {
            return temp_list;
        }
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
                    //Debug.Log("bonjour monsieur MQTT !");
                }
            }

            for (int j = 0; j < id_to_parse.Length; j++)
            {

                _cacheGlobalTopic.Send(
                //{"dttp": null, "data": 22.5, "t": "2017-05-15T06:47:42Z", "id": "zwave1/:3260679919/:2/:/infos.1/:/1/:/1"}
                new GlobalMessage { dttp = "", data = 22.5, t = "2017-05-15T06:47:42Z", id = id_to_parse[j] },

                false,
                QualityOfServiceEnum.ExactlyOnce);
            }



        }

        void onMqttMessageDelivered_GlobalTopic(string messageId)
        {
            //Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            //Debug.Log("message just arrived");
            GlobalMessage receive_obj;

            for (int i = 0; i < id_to_parse.Length; i++)
            {
                temp_list.Add(0.0);
            }

            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {
                //Debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);


                //parse intersting message
                for (int i = 0; i < id_to_parse.Length; i++)
                {
                    if (json.Contains(id_to_parse[i]) == true)
                    {
                        receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                        temp_list[i] = receive_obj.data;
                        //Debug.Log("Value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                        mesage_receive = true;
                    }
                }
            }

            else
                Debug.LogWarning("message arrived, but failed JSON conversion");

            if (mesage_receive)
            {
                double temp_value = 0.0;
                //print("temp value before addition : " + temp_value);
                int j = 0;
                for (; j < id_to_parse.Length; j++)
                {
                    temp_value += temp_list[j];
                }
                //print("temp value after addition : " + temp_value);
                temp_value /= id_to_parse.Length;
                temp_list.Add(temp_value);

                for (int i = 0; i < lux_list_activity.Length; i++)
                {
                    //Debug.Log("temp value : " + temp_value + " ; mqtt value : " + lux_list_activity[i]);
                    if (temp_value <= lux_list_activity[i])
                    {
                        send_notif = new Notifications("Tips_light");
                        queuing.sendNotification(send_notif);
                        Debug.Log("Activity number " + (i + 1) + " " + lux_list_activity[i] + " detected");
                        i = lux_list_activity.Length;
                    }
                    else if (temp_value > lux_list_activity[lux_list_activity.Length - 1])
                    {
                        //content.text = "You consume some much that you have kill the earth !";
                    }
                }
            }

        }

        void onMqttSubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            //Debug.Log("subscription successful");

            /*Debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
				" 'Best effort' QOS does not get delivery verification from broker.  " +
				"'Best effort' is however the quickest and dirtiest way to send a message.")*/
            ;
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
