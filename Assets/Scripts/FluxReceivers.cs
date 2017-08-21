
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


namespace HG.iot.mqtt.example
{
    public class FluxReceivers : MonoBehaviour
    {
        public string[] id_to_parse;

        public string id_before_charger;
        public string id_after_charger;
        private double value_before_charger;
        private double value_after_charger;

        public Sprite sprite_positive;
        public Sprite sprite_negative;

        public Image arrow;

        public Text text_value;
        private float temp_val;

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


            for (int j = 0; j < id_to_parse.Length; j++)
            {

                _cacheGlobalTopic.Send(
                //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
                new GlobalMessage { dttp = "this is text", data = 123, t = "2017-05-15T06:47:42Z", id = id_to_parse[j] },

                false,
                QualityOfServiceEnum.AtLeastOnce);
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
                prod_list.Add(0.0F);
            }

            // Debug.Log("Message arrived on GlobalTopic");
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message");

            if (!message.JSONConversionFailed)
            {

                string json = JsonUtility.ToJson(message);

                if (!id_before_charger.Contains("null") && json.Contains(id_before_charger) == true)
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    value_before_charger = receive_obj.data;
                    //Debug.Log("Batt power value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);

                }
                else if (!id_after_charger.Contains("null") && json.Contains(id_after_charger) == true)
                {
                    receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                    value_after_charger = receive_obj.data;
                    //Debug.Log("After bat power value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                }

                else
                {
                    //parse intersting message
                    for (int i = 0; i < id_to_parse.Length; i++)
                    {
                        if (json.Contains(id_to_parse[i]) == true)
                        {
                            receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                            prod_list[i] = receive_obj.data;
                            //Debug.Log("Global value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);
                        }
                    }
                }
   
            }

            else
                Debug.LogWarning("message arrived, but failed JSON conversion");

            temp_val = 0.0f;

            if (id_to_parse.Length == 0)
            {
                if(value_before_charger >= 0)
                {
                    temp_val =(float) (value_before_charger - value_after_charger);
                    //Debug.Log("temp value positiv : " + temp_val+" ; before : " +value_before_charger+" ; after : "+value_after_charger );
                }
                else
                {
                    temp_val = (float)(-1*(Math.Abs(value_before_charger) + value_after_charger));
                    //Debug.Log("temp value negativ : " + temp_val + " ; before : " + value_before_charger + " ; after : " + value_after_charger);
                }
            }
            else
            {
                int j = 0;
                for (; j < id_to_parse.Length; j++)
                {
                    temp_val += (float)prod_list[j];
                }
            }
            //print("power value : " + prod_slider.value);


            if (temp_val >= 0)
            {
                arrow.sprite = sprite_positive;
            }
            else
            {
                arrow.sprite = sprite_negative;
            }
            temp_val = Math.Abs(temp_val);
            text_value.text = (temp_val / 1000).ToString("F1") + " kW";

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


