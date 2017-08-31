using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Queing_notification : MonoBehaviour {

    public List<GameObject> prefab_list;
    public GameObject default_tips;
    
    public Toggle alarmButton;
    public Image image;
    public Text text_notif;

    public Transform transform_parent;

    private Queue<Notifications> notificationQueue;
    private int notif_num;
    private Notifications actual_notif;

    private GameObject actual_go;
    private string time_string;
    private TimeSpan time_diff;
    
    private void Start()
    {
        notificationQueue = new Queue<Notifications>();
        notif_num = notificationQueue.Count;
    }

    public void ClickAlarmOn(bool value_toggle)
    {
        //Debug.Log("click on alarm");
        if (value_toggle)
        {
            notif_num--;
            updateNotif();
            if (!(notif_num < 0))
            {
                dequeNewTips();
            }
            else
            {
                notif_num = 0;
                actual_go = (GameObject)Instantiate(default_tips, transform_parent);
                actual_go.GetComponentInChildren<Button>().onClick.AddListener(closeActualPopup);
            }

        }
        else
        {
            actual_go.SetActive(false);
            Destroy(actual_go);
        }

    }

    public void closeActualPopup()
    {
        actual_go.SetActive(false);
        Destroy(actual_go);
        if (notificationQueue.Count > 0)
        {
            notif_num--;
            updateNotif();
            dequeNewTips();
        }
        else
        {
            notif_num = 0;
            updateNotif();
            alarmButton.isOn = false;
        }
    }

    public void sendNotification(Notifications new_notif)
    {
        //Debug.Log("new notification : " + new_notif.ActualTime.ToString()+" ; "+new_notif.Prefab_key.ToString());
        notificationQueue.Enqueue(new_notif);
        notif_num++;
        updateNotif();

    }

    private void dequeNewTips()
    {
        actual_notif = notificationQueue.Dequeue();

        for (int i = 0; i < prefab_list.Count; i++)
        {
            if (string.Compare(prefab_list[i].name, actual_notif.Prefab_key) == 0)
            {
                actual_go = (GameObject)Instantiate(prefab_list[i],transform_parent);
                actual_go.GetComponentInChildren<Button>().onClick.AddListener(closeActualPopup);

                time_diff = DateTime.Now - actual_notif.ActualTime;
                if (time_diff.Days > 1)
                {
                    time_string = time_diff.Days.ToString() + " days ago : ";
                }
                else if (time_diff.Hours > 1)
                {
                    time_string = time_diff.Hours.ToString() + " hours ago : ";
                }
                else if(time_diff.Minutes > 1) 
                {
                    time_string = time_diff.Minutes.ToString() + " minutes ago :";
                }else
                {
                    time_string = time_diff.Seconds.ToString() + " seconds ago :";
                }

                actual_go.GetComponentInChildren<Text>().text = time_string;
            }
        }
    }
    private void updateNotif()
    {
        if (notif_num <= 0)
        {
            image.gameObject.SetActive(false);
        }
        else
        {
            image.gameObject.SetActive(true);
            text_notif.text = notif_num.ToString();
        }
    }
}