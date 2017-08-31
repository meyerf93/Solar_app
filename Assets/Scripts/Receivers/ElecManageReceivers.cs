using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HG.iot.mqtt.example
{
    public class ElecManageReceivers : MonoBehaviour
    {
        public Queing_notification notif;
        public string[] id_produce;
        public string[] id_consume;
        public string id_before_bat;
        public string id_after_bat;

        public double prod_max;
        public double threshold_prod_low;
        public double threshold_prod_high;

        private List<double> prod_list = new List<double>();
        private List<double> conso_list = new List<double>();
        private double value_prod = 0;
        private double value_conso = 0;
        private double value_before_bat = 0;
        private double value_after_bat = 0;
        private double value_battery = 0;

        private Notifications test = null;
        private double temp_value;

        private bool right_message = false;

        private TimeSpan old_time_prod_more;
        public int time_wait_prod_more;
        private bool flag_prod_more = false;

        private TimeSpan old_time_prod_less;
        public int time_wait_prod_less;
        private bool flag_prod_less = false;

        private TimeSpan old_time_conso_more;
        public int time_wait_conso_more;
        private bool flag_conso_more = false;

        private TimeSpan old_time_conso_less;
        public int time_wait_conso_less;
        private bool flag_conso_less = false;

        ITopic _cacheGlobalTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            //debug.Log("onMqttReady invoked");
            //debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE", topic.Filter));

            tests(topic);
        }

        // Topic.SimpleNotifications=FALSE
        void onMqttReady_GlobalTopic(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            //debug.Log("onMqttReady_GlobalTopic invoked");
            //debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to FALSE", topic.Filter));
            //debug.Log("SimpleNotifications=FALSE give you the flexibility of receiving messages from various topics within the same receiver script");
            //debug.Log("Every notification will be in the format '[notification-method]_[topic-filter]'");

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
                    //debug.Log("Let's try subscribing to this topic without connecting to broker first....");
                    topic.Subscribe();
                }
                catch (OperationCanceledException ocex)
                {
                    Debug.LogError(ocex.ToString() + "Performing actions that require an active connection will throw an 'OperationCancelledException'");
                }
            }

            //debug.Log("try so send a message for the car");
            /*_cacheGlobalTopic.Send(
            //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
            new GlobalMessage { dttp = "this is text", data = DateTime.Now.Hour, t = "2017-05-15T06:47:42Z", id = id_to_parse },
            false,
            QualityOfServiceEnum.AtLeastOnce);*/

        }

        void onMqttMessageDelivered_GlobalTopic(string messageId)
        {
            //debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            //debug.Log("message just arrived");
            GlobalMessage receive_obj;

            //debug.Log("Message arrived on CarMessage");
            //debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {
                //debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);

                if (json.Contains(id_after_bat))
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    value_after_bat = receive_obj.data;
                    right_message = true;
                    //Debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                }
                else if (json.Contains(id_before_bat))
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    value_before_bat = receive_obj.data;
                    //Debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                    right_message = true;
                }
                else
                {
                    for (int i = 0; i < id_consume.Length; i++)
                    {
                        if(conso_list.Count < id_consume.Length) conso_list.Add(0.0F);
                        if (json.Contains(id_consume[i]))
                        {
                            receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                            conso_list[i] = receive_obj.data;
                            //Debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                            right_message = true;
                        }
                    }
                    for (int i = 0; i < id_produce.Length; i++)
                    {
                        if(prod_list.Count < id_produce.Length) prod_list.Add(0.0f);
                        if (json.Contains(id_produce[i]))
                        {
                            receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                            prod_list[i] = receive_obj.data;
                            //Debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                            right_message = true;
                        }
                    }
                }
                if(right_message)
                {
                    value_prod = 0;
                    for (int j = 0; j < id_produce.Length; j++) value_prod += prod_list[j];

                    value_conso = 0;
                    for (int j = 0; j < id_consume.Length; j++) value_conso += conso_list[j];

                    if (value_before_bat > 0)
                    {
                        if (value_after_bat > 0) // +         -         +
                            value_battery = value_before_bat - value_after_bat;
                        else
                            value_battery = value_before_bat;
                                                // +
                    }
                    else
                    {
                        if (value_after_bat > 0) // -       +         +
                            value_battery = value_before_bat + value_after_bat;
                        else
                            value_battery = value_before_bat;
                                                // -
                    }
                    //Debug.Log("value before bat : " + value_before_bat);
                    //Debug.Log("value after bat : " + value_after_bat);
                    //Debug.Log("value of battery : " + value_battery);

                    if (value_after_bat > 0) value_conso += value_after_bat;
                    if (value_battery > 0) value_conso += value_battery;

                    //Debug.Log("value of conso : " + value_conso);
                    //Debug.Log("value of prod : " + value_prod);

                    if (value_prod == value_conso)
                    {
                        if (value_conso > prod_max / 100 * threshold_prod_high)
                        {
                            if ((DateTime.Now.TimeOfDay - old_time_conso_more).Minutes > time_wait_conso_more || flag_conso_more == false)
                            {
                                //Debug.Log("Sends cono down, value conso : " + value_conso + " ; value prod : " + value_prod);
                                old_time_conso_more = DateTime.Now.TimeOfDay;
                                flag_conso_more = true;
                                test = new Notifications("Tips_consu_down");
                                notif.sendNotification(test);
                            }
                            flag_conso_less = false;
                        }
                        else if (value_conso < prod_max / 100 * threshold_prod_low)
                        {
                            if ((DateTime.Now.TimeOfDay - old_time_prod_less).Minutes > time_wait_prod_less || flag_prod_less == false)
                            {
                                //Debug.Log("Sends produ down, value conso : " + value_conso + " ; value prod : " + value_prod);
                                old_time_prod_less = DateTime.Now.TimeOfDay;
                                flag_prod_less = true;
                                test = new Notifications("Tips_produ_down");
                                notif.sendNotification(test);
                            }
                            flag_prod_more = false;
                        }
                    }
                    else if (value_prod > value_conso)
                    {
                        if ((DateTime.Now.TimeOfDay - old_time_prod_more).Minutes > time_wait_prod_more || flag_prod_more == false)
                        {
                            //Debug.Log("Sends produ up, value conso : " + value_conso + " ; value prod : " + value_prod);
                            old_time_prod_more = DateTime.Now.TimeOfDay;
                            flag_prod_more = true;
                            test = new Notifications("Tips_produce_up");
                            notif.sendNotification(test);
                        }
                        flag_prod_less = false;
                    }
                    else
                    {
                        if ((DateTime.Now.TimeOfDay - old_time_conso_less).Minutes > time_wait_conso_less || flag_conso_less == false)
                        {
                            //Debug.Log("Sends conso up, value conso : " + value_conso + " ; value prod : " + value_prod);
                            old_time_conso_less = DateTime.Now.TimeOfDay;
                            flag_conso_less = true;
                            test = new Notifications("Tips_consu_up");
                            notif.sendNotification(test);
                        }
                        flag_conso_more = false;
                    }

                    right_message = false;
                }
              
            }
            else Debug.LogWarning("message arrived, but failed JSON conversion");
        }

        void onMqttSubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            //debug.Log("subscription successful");

            /*debug.Log("Let's send a message with a QOS of 'at least once' or 'exactly once'. " +
                " 'Best effort' QOS does not get delivery verification from broker.  " +
                "'Best effort' is however the quickest and dirtiest way to send a message.");*/
        }

        void onMqttSubscriptionFailure_GlobalTopic(SubscriptionResponse response)
        {
            //debug.Log("subscription failed");
        }

        void onMqttUnsubscriptionSuccess_GlobalTopic(SubscriptionResponse response)
        {
            //debug.Log("unsubscription successful");
        }

        void onMqttUnsubscriptionFailure_GlobalTopic(SubscriptionResponse response)
        {
            //debug.Log("unsubscription failed");
        }

        void onMqttConnectSuccess_GlobalTopic(ConnectionResult response)
        {
            //debug.Log("you are connected to broker");
        }

        void onMqttConnectFailure_GlobalTopic(ConnectionResult response)
        {
            //debug.Log("connection to broker failed");
        }

        void onMqttConnectLost_GlobalTopic(ConnectionResult response)
        {
            //debug.Log("connection to broker lost");
        }

        void onMqttReconnect_GlobalTopic(ConnectionResult response)
        {
            //debug.Log("broker has reconnected");
        }
    }
}
