using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications_Receivers : MonoBehaviour {

    public Queing_notification notif;

    private Notifications test;
    private int send_one = 13;

    // Use this for initialization
    void Start () {
        test = new Notifications("Tips_light");   
	}
	
	// Update is called once per frame
	void Update () {
        if (send_one > 0)
        {
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_buy_expen");

            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_consu_up");
            notif.sendNotification(test);
            send_one--;


            test = new Notifications("Tips_sell_cheap");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_consu_down");
            notif.sendNotification(test);
            send_one--;


            test = new Notifications("Tips_sell_expen");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_produ_down");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_buy_cheap");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_produce_up");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_unplug");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_speed-");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_plug");
            notif.sendNotification(test);
            send_one--;

            test = new Notifications("Tips_speed+");
            notif.sendNotification(test);
            send_one--;
        }
    }
}
