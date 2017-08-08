using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class PriceTipsReceivers : MonoBehaviour
    {
        public Queing_notification notif;
        public string id_to_parse;

        private Notifications test = null;
        private double temp_value;

        private TimeSpan old_time_buy;
        public int time_wait_min_buy;

        private TimeSpan old_time_sell;
        public int time_wait_min_sell;

        private bool flag_buy_cheap = false;
        private bool flag_sell_cheap = false;
        private bool flag_buy_expen = false;
        private bool flag_sell_expen = false;

        public int good_sell_begin;
        public int good_sell_end;
        public int bad_sell_begin_1;
        public int bad_sell_end_1;
        public int bad_sell_begin_2;
        public int bad_sell_end_2;
        public int good_buy_begin_1;
        public int good_buy_end_1;
        public int good_buy_begin_2;
        public int good_buy_end_2;
        public int bad_buy_begin;
        public int bad_buy_end;

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
            _cacheGlobalTopic.Send(
            //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
            new GlobalMessage { dttp = "this is text", data = DateTime.Now.Hour, t = "2017-05-15T06:47:42Z", id = id_to_parse },
            false,
            QualityOfServiceEnum.AtLeastOnce);

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


                //parse intersting message
                if (json.Contains(id_to_parse) == true)
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    temp_value = receive_obj.data;

                    if(temp_value >= 0.0) // j'achète
                    {
                        if (DateTime.Now.TimeOfDay.Hours >= good_buy_begin_1 && DateTime.Now.TimeOfDay.Hours <= good_buy_end_1) //bonne periode
                        {
                            if ((DateTime.Now.TimeOfDay - old_time_buy).Seconds > time_wait_min_buy || flag_buy_cheap == false)
                            {
                                old_time_buy = DateTime.Now.TimeOfDay;
                                flag_buy_cheap = true;
                                test = new Notifications("Tips_buy_cheap");
                                notif.sendNotification(test);
                            }
                        }
                        else if (DateTime.Now.TimeOfDay.Hours >= bad_buy_begin && DateTime.Now.TimeOfDay.Hours <= bad_buy_end) // mauvaise bonne periode
                        {
                            if((DateTime.Now.TimeOfDay - old_time_buy).Seconds > time_wait_min_buy || flag_buy_expen == false)
                            {
                                old_time_buy = DateTime.Now.TimeOfDay;
                                flag_buy_expen = true;
                                test = new Notifications("Tips_buy_expen");
                                notif.sendNotification(test);
                            }
                        }
                        else if (DateTime.Now.TimeOfDay.Hours >= good_buy_begin_2 && DateTime.Now.TimeOfDay.Hours <= good_buy_end_2) //bonne periode
                        {
                            if ((DateTime.Now.TimeOfDay - old_time_buy).Seconds > time_wait_min_buy || flag_buy_cheap == false)
                            {
                                old_time_buy = DateTime.Now.TimeOfDay;
                                flag_buy_cheap = true;
                                test = new Notifications("Tips_buy_cheap");
                                notif.sendNotification(test);
                            }
                        }
                        flag_sell_cheap = false;
                        flag_sell_expen = false;
                    }
                    else // je vends
                    {
                        if (DateTime.Now.TimeOfDay.Hours >= bad_sell_begin_1 && DateTime.Now.TimeOfDay.Hours <= bad_sell_end_1)
                        {
                            if ((DateTime.Now.TimeOfDay - old_time_sell).Seconds > time_wait_min_sell || flag_sell_cheap == false)
                            {
                                old_time_sell = DateTime.Now.TimeOfDay;
                                flag_sell_cheap = true;
                                test = new Notifications("Tips_sell_cheap");
                                notif.sendNotification(test);  
                            }
                        }
                        else if (DateTime.Now.TimeOfDay.Hours >= good_sell_begin && DateTime.Now.TimeOfDay.Hours <= good_sell_end)
                        {
                            if((DateTime.Now.TimeOfDay - old_time_sell).Seconds > time_wait_min_sell || flag_sell_expen == false)
                            {
                                old_time_sell = DateTime.Now.TimeOfDay;
                                flag_sell_expen = true;
                                test = new Notifications("Tips_sell_expen");
                                notif.sendNotification(test);
                            }
                        }
                        else if (DateTime.Now.TimeOfDay.Hours >= bad_sell_begin_2 && DateTime.Now.TimeOfDay.Hours <= bad_sell_end_2)
                        {
                            if ((DateTime.Now.TimeOfDay - old_time_sell).Seconds > time_wait_min_sell || flag_sell_cheap == false)
                            {
                                old_time_sell = DateTime.Now.TimeOfDay;
                                flag_sell_cheap = true;
                                test = new Notifications("Tips_sell_cheap");
                                notif.sendNotification(test);
                            }
                        }
                        flag_buy_cheap = false;
                        flag_buy_expen = false;
                    }
                    //debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);

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
