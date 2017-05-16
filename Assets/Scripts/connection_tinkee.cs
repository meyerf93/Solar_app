/*using UnityEngine;
using System.Collections;

using uPLibrary.Networking.M2Mqtt;
using System;
using System.Runtime.InteropServices;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;

public class connection_tinkee : MonoBehaviour
{

    //http://tdoc.info/blog/2014/11/10/mqtt_csharp.html
    private MqttClient4Unity client;

    public string brokerHostname = null;
    public int brokerPort = 1883;
    /*public string userName = null;
    public string password = null;
    public string [] topic = null;
    public string value = null;

    private Queue msgq = new Queue();

    string lastMessage = null;


    byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                         MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                         MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                         MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                         MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE};

    // Use this for initialization
    void Start()
    {
        if (brokerHostname != null)// && userName != null) && password != null)
        {
            Connect();
            print("Client connected");
            client.Subscribe(topic[0]);//,qosLevels);
            client.Subscribe(topic[1]);//,qosLevels);
            client.Subscribe(topic[2]);//,qosLevels);
            client.Subscribe(topic[3]);//,qosLevels);
            client.Subscribe(topic[4]);//,qosLevels);
            /*for (int i= 0; i<topic.Length; i++)
            {
                
                print("topic : " + topic[i]+",QOS : " + qosLevels[i]);
            }
            print("All topic subscribe subscribed");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //client.Publish(topic, System.Text.Encoding.ASCII.GetBytes("ni!"));
        if (client.Count() > 0)
        {
            print("client count : " + client.Count());
            Debug.Log("received :" + client.ReceiveEvent());
        }
    }

    void OnGUI()
    {
        if (msgq != null && msgq.Count > 0)
        {
            if (lastMessage == null)
            {
                lastMessage = (string)msgq.Dequeue();
            }
            else
            {
                lastMessage = (string)msgq.Dequeue();
                lastMessage = null;
            }
        }
        GUILayout.Label(lastMessage);
    }

    public void Connect()
    {
        client = new MqttClient4Unity(brokerHostname, brokerPort, false, null);
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);//, userName,password);
    }

    public void Publish(string _topic, string msg)
    {
        client.Publish(
            _topic, Encoding.UTF8.GetBytes(msg),
            MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
    }
}*/


