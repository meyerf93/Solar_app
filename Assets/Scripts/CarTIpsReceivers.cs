using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class CarTIpsReceivers : MonoBehaviour
    {

        public Queing_notification notif;

        private Notifications test;
        private int send_one = 13;

        public string id_state;

        const string constPLUG_1 = "Plugged on WallBox and Locked and EV";
        const string constPLUG_2 = "Plugged on WallBox and EV";

        private string temp_state;

        public string id_current;
        public double max_current_ma;
        public double threshould_low;
        public double threshould_hight;
        private double temp_current;

        ITopic _cacheCarTopic = null;

        ITopic _cacheGlobalTopic = null;

        // Topic.SimpleNotifications=TRUE
        void onMqttReady(ITopic topic)
        {
            _cacheGlobalTopic = topic;

            _cacheCarTopic = topic;

            //Debug.Log("onMqttReady invoked");
            //Debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to TRUE",topic.Filter));

            tests(topic);
        }

        void onMqttReady_CarTopic(ITopic topic)
        {
            _cacheCarTopic = topic;

            //debug.Log("onMqttReady_CarTopic invoked");
            //debug.Log(string.Format("'{0}' topic's SimpleNotifications are set to FALSE", topic.Filter));
            //debug.Log("SimpleNotifications=FALSE give you the flexibility of receiving messages from various topics within the same receiver script");
            //debug.Log("Every notification will be in the format '[notification-method]_[topic-filter]'");

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

            _cacheCarTopic.Send(
            //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
            new CarMessage { dttp = "this is text", data = constPLUG_1, t = "2017-05-15T06:47:42Z", id = id_state },
            false,
            QualityOfServiceEnum.AtLeastOnce);

            _cacheCarTopic.Send(
            //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
            new CarMessage { dttp = "this is text", data = "Plugged on WallBox", t = "2017-05-15T06:47:42Z", id = id_state },
            false,
            QualityOfServiceEnum.AtLeastOnce);

            _cacheGlobalTopic.Send(
            //{"dttp": null, "data": 22.5, "t": "2017-05-15T06:47:42Z", "id": "zwave1/:3260679919/:2/:/infos.1/:/1/:/1"}
            new GlobalMessage { dttp = "this is text", data = 22.5, t = "2017-05-15T06:47:42Z", id = id_current },
            false,
            QualityOfServiceEnum.ExactlyOnce);

            _cacheGlobalTopic.Send(
            //{"dttp": null, "data": 22.5, "t": "2017-05-15T06:47:42Z", "id": "zwave1/:3260679919/:2/:/infos.1/:/1/:/1"}
            new GlobalMessage { dttp = "this is text", data = 12000, t = "2017-05-15T06:47:42Z", id = id_current },
            false,
            QualityOfServiceEnum.ExactlyOnce);



        }

        void onMqttMessageDelivered_CarTopic(string messageId)
        {
            //debug.Log("message delivered to broker");
        }

        void onMqttMessageDelivered_GlobalTopic(string messageId)
        {
            //Debug.Log("message delivered to broker");
        }

        void onMqttMessageArrived_CarTopic(CarMessage message)
        {
            //debug.Log("message just arrived");
            CarMessage receive_obj;


            //debug.Log("Message arrived on CarMessage");
            //debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {
                //debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);


                //parse intersting message
                if (json.Contains(id_state) == true)
                {
                    receive_obj = JsonUtility.FromJson<CarMessage>(json);
                    temp_state = receive_obj.data;
                    //debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                    if (temp_state.Contains(constPLUG_1) || temp_state.Contains(constPLUG_2))
                    {
                        test = new Notifications("Tips_plug");
                        notif.sendNotification(test);
                    }
                    else
                    {
                        test = new Notifications("Tips_unplug");
                        notif.sendNotification(test);
                    }
                }
            }
            else
                Debug.LogWarning("message arrived, but failed JSON conversion");
        }


        void onMqttMessageArrived_GlobalTopic(GlobalMessage message)
        {
            //Debug.Log("message just arrived");
            GlobalMessage receive_obj;


            //Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {
                //Debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);


                //parse intersting message

                if (json.Contains(id_current) == true)
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    temp_current = receive_obj.data;
                    //Debug.Log("Value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                    if(temp_current >= max_current_ma / 100 * threshould_hight)
                    {

                        test = new Notifications("Tips_speed+");
                        notif.sendNotification(test);
                    }
                    else if (temp_current <= max_current_ma / 100 * threshould_low)
                    {
                        test = new Notifications("Tips_speed-");
                        notif.sendNotification(test);
                    }
                }
            }
            else
                Debug.LogWarning("message arrived, but failed JSON conversion");
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

        void onMqttSubscriptionFailure_CarTopic(SubscriptionResponse response)
        {
            //debug.Log("subscription failed");
        }

        void onMqttUnsubscriptionSuccess_CarTopic(SubscriptionResponse response)
        {
            //debug.Log("unsubscription successful");
        }

        void onMqttUnsubscriptionFailure_CarTopic(SubscriptionResponse response)
        {
            //debug.Log("unsubscription failed");
        }

        void onMqttConnectSuccess_CarTopic(ConnectionResult response)
        {
            //debug.Log("you are connected to broker");
        }

        void onMqttConnectFailure_CarTopic(ConnectionResult response)
        {
            //debug.Log("connection to broker failed");
        }

        void onMqttConnectLost_CarTopic(ConnectionResult response)
        {
            //debug.Log("connection to broker lost");
        }

        void onMqttReconnect_CarTopic(ConnectionResult response)
        {
            //debug.Log("broker has reconnected");
        }
    }
}