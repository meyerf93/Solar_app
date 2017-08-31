using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace HG.iot.mqtt.example
{
    public class MapLightReceivers : MonoBehaviour
    {
        public string[] id_to_parse;
        public Image[] list_image;

        public bool debug_mode_on;

        public Sprite low_sprite;
        public Sprite mid_sprite;
        public Sprite high_sprite;

        private List<int> light_mean = new List<int>();

        public int low_threshould;
        public int hight_threshould;
        private bool image_modified = false;
        private Sprite newSprite;
        private bool image_is_zero = false;

        ITopic _cacheGlobalTopic = null;

        void Start()
        {
            //Debug.Log("number of id too parse : " + id_to_parse.Length);
            for (int i = 0; i < id_to_parse.Length; i++)
            {
                light_mean.Add(0);
            }
            //Debug.Log("count light_mean : " + light_mean.Count + " id : " + id_to_parse[0]);
        }

        

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
                for (int j = 0; j < id_to_parse.Length; j++)
                {
                    _cacheGlobalTopic.Send(
                    //{"dttp": null, "data": 22.5, "t": "2017-05-15T06:47:42Z", "id": "zwave1/:3260679919/:2/:/infos.1/:/1/:/1"}
                    new GlobalMessage { dttp = "this is text", data = 45, t = "2017-05-15T06:47:42Z", id = id_to_parse[j] },

                    false,
                    QualityOfServiceEnum.ExactlyOnce);
                    //Debug.Log("test the topic and send a message j : " + j);
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
            //Debug.Log("Note that the message parameter in the arrival notification is strong typed to that of the topic's message")

            if (!message.JSONConversionFailed)
            {
                //Debug.Log(JsonUtility.ToJson(message));
                string json = JsonUtility.ToJson(message);
                //Debug.Log(id_to_parse);
                //parse intersting message
                for (int j = 0; j < id_to_parse.Length; j++)
                {
                    if (json.Contains(id_to_parse[j]))
                    {
                        receive_obj = JsonUtility.FromJson<GlobalMessage>(json);
                        light_mean[j] = (int)receive_obj.data;
                        //Debug.Log("Value of json object : data : " + receive_obj.data + ", t : " + receive_obj.t + ", id : " + receive_obj.id);

                        float mean = 0.0f;
                        //Debug.Log("count light_mean  value : " + light_mean.Count);
                        for (int i = 0; i < light_mean.Count; i++)
                        {
                            //Debug.Log("value of mean calcul : " + mean);
                            mean += light_mean[i];
                        }
                        mean = mean / light_mean.Count;

                        if (mean == 0)
                        {
                            //Debug.Log("zero : " + mean);
                            image_is_zero = true;
                            image_modified = false;
                        }
                        else if (mean <= low_threshould)
                        {
                            //Debug.Log("low : " + mean);
                            image_modified = true;
                            image_is_zero = false;
                            newSprite = low_sprite;
                        }
                        else if (low_threshould <= mean && mean <= hight_threshould)
                        {
                            //Debug.Log("mid : " + mean);
                            image_modified = true;
                            image_is_zero = false;
                            newSprite = mid_sprite;
                        }
                        else if (mean >= hight_threshould)
                        {
                            //Debug.Log("high : " + mean);
                            image_modified = true;
                            image_is_zero = false;
                            newSprite = high_sprite;
                        }

                        if (image_is_zero)
                        {
                            for (int i = 0; i < list_image.Length; i++)
                            {
                                Color oldColor = list_image[i].color;
                                list_image[i].color = new Color(100.0f, 100.0f, 100.0f, 0.0f);
                            }
                        }
                        else if (image_modified)
                        {
                            //Debug.Log("change image detected");
                            for (int i = 0; i < list_image.Length; i++)
                            {
                                //Debug.Log("sprite before : " + list_image[i].sprite);
                                list_image[i].sprite = newSprite;
                                //Debug.Log("sprite after : " + list_image[i].sprite);
                                list_image[i].color = new Color(100.0f, 100.0f, 100.0f, 100.0f);
                            }
                        }
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
    }
}
