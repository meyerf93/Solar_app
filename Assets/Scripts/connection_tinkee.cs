using UnityEngine;
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
    public string password = null;*/
    public string topic = null;
    public string value = null;

    private Queue msgq = new Queue();

    string lastMessage = null;

    // Use this for initialization
    void Start()
    {
        if (brokerHostname != null)// && userName != null) && password != null)
        {
            Connect();
            client.Subscribe(topic);
            print("client connected and subscribed");
            client.Publish(topic, System.Text.Encoding.ASCII.GetBytes("{\"cmd\":\"knx1/:1.2.26/:/dim.1\",\"mdl\":\"knx1\",\"value\":"+value+"}"));
            print("client send data");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //client.Publish(topic, System.Text.Encoding.ASCII.GetBytes("ni!"));
        while (client.Count() > 0)
        {
            string s = client.Receive();
            msgq.Enqueue(s);
            Debug.Log("received :" + s);
        }

        if (Input.GetMouseButtonDown(0) == true)
        {
            client.Publish(topic, System.Text.Encoding.ASCII.GetBytes("nice click!"));
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
}


