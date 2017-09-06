using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class LuxReceivers : MonoBehaviour
    {
        public Queing_notification notif;

        public string[] id_lux;
        public string[] id_light;

        public bool debug_mode_on;

        private Notifications test = null;

        public double lux_threshold;

        private TimeSpan old_time_lux;
        public int time_wait_lux;
        private bool flag_lux = false;

        private bool light_is_on = false;
        private bool right_message = false;

        private List<double> lux_list = new List<double>();
        private List<bool> light_list = new List<bool>();
        private double mean_luxe = 0;

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
                    //Debug.Log("bonjour monsieur MQTT !");
                }
            }

            if (debug_mode_on)
            {

                for (int j = 0; j < id_lux.Length; j++)
                {
                    _cacheGlobalTopic.Send(
                    //{"dttp": null, "data": 22.5, "t": "2017-05-15T06:47:42Z", "id": "zwave1/:3260679919/:2/:/infos.1/:/1/:/1"}
                    new GlobalMessage { dttp = "", data = 22.5, t = "2017-05-15T06:47:42Z", id = id_lux[j] },
                    false,
                    QualityOfServiceEnum.ExactlyOnce);
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
            GlobalMessage receive_obj;

            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {
                right_message = false;
                //Debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);

                for (int i = 0; i < id_lux.Length; i++)
                {
                    if (lux_list.Count < id_lux.Length) lux_list.Add(0.0F);
                    if (json.Contains(id_lux[i]) == true)
                    {
                        receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                        lux_list[i] = receive_obj.data;
                        //Debug.Log("Value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                        right_message = true;
                    }
                }

                for (int i = 0; i < id_light.Length; i++)
                {
                    if (light_list.Count < id_light.Length) light_list.Add(false);
                    if (json.Contains(id_light[i]) == true)
                    {
                        receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                        if (receive_obj.data > 0) light_list[i] = true;
                        //Debug.Log("Value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                        right_message = true;
                    }
                }
            }

            else
                Debug.LogWarning("message arrived, but failed JSON conversion");

            if (right_message)
            {
                for (int j = 0; j < id_lux.Length; j++) mean_luxe += lux_list[j];
                mean_luxe /= lux_list.Count;

                light_is_on = false;
                for (int j = 0; j < id_light.Length; j++) if (light_list[j]) light_is_on = true;

                if (mean_luxe >= lux_threshold && light_is_on)
                {
                    //Debug.Log("old time " + old_time_lux + " ; actual time " + DateTime.Now.TimeOfDay+ " time wait : " + time_wait_lux);
                    if ((DateTime.Now.TimeOfDay - old_time_lux).Minutes > time_wait_lux || flag_lux == false)
                    {
                        //Debug.Log("Send notification");
                        //Debug.Log("old time " + old_time_lux + " ; actual time " + DateTime.Now.TimeOfDay + " time wait : " + time_wait_lux);

                        old_time_lux = DateTime.Now.TimeOfDay;
                        flag_lux = true;
                        test = new Notifications("Tips_light");
                        notif.sendNotification(test);
                    }
                }
                else flag_lux = false;
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
