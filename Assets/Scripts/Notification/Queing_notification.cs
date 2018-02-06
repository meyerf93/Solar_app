using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Queing_notification : MonoBehaviour {

    public List<GameObject> prefab_list;
    public List<GameObject> prefab_mini_list;
    public GameObject List_tips;
    public GameObject default_tips;

    public Boolean Generate_defautl_tips;

    public float Space_change;
    
    public Toggle alarmButton;
    public Image image;
    public Text text_notif;

    public Transform transform_parent;

    private List<Notifications> notificationList;
    private int notif_num;
    private Notifications actual_notif;

    private GameObject actual_go;
    private string time_string;
    private TimeSpan time_diff;

    private Notifications temp_notif;

    private void Start()
    {
        notificationList = new List<Notifications>();
        notif_num = notificationList.Count;
        if (Generate_defautl_tips) generateDefaultTips();
    }


    public void generateDefaultTips()
    {
        Notifications test = null;
        test = new Notifications("Tips_light");
        this.sendNotification(test);
        test = new Notifications("Tips_buy_cheap");
        this.sendNotification(test);
        test = new Notifications("Tips_buy_expen");
        this.sendNotification(test);
        test = new Notifications("Tips_sell_cheap");
        this.sendNotification(test);
        test = new Notifications("Tips_sell_expen");
        this.sendNotification(test);
        test = new Notifications("Tips_consu_down");
        this.sendNotification(test);
        test = new Notifications("Tips_consu_up");
        this.sendNotification(test);
        test = new Notifications("Tips_produ_down");
        this.sendNotification(test);
        test = new Notifications("Tips_produce_up");
        this.sendNotification(test);
        test = new Notifications("Tips_unplug");
        this.sendNotification(test);
        test = new Notifications("Tips_speed-");
        this.sendNotification(test);
        test = new Notifications("Tips_plug");
        this.sendNotification(test);
        test = new Notifications("Tips_speed+");
        this.sendNotification(test);
    }

    public void ClickAlarmOn(bool value_toggle)
    {
        //Debug.Log("click on alarm");
        if (value_toggle)
        {
            if (!(notif_num <= 0))
            {
                showTipsList();
            }
            else
            {
                notif_num = 0;
                actual_go = (GameObject)Instantiate(default_tips, transform_parent);
                actual_go.GetComponentInChildren<Button>().onClick.AddListener(delegate { closeActualPopup(true); });
            }
        }
        else
        {
            actual_go.SetActive(false);
            Destroy(actual_go);
        }
    }

    public void closeActualPopup(bool active_alarm)
    {
        actual_go.SetActive(false);
        Destroy(actual_go);
        if(active_alarm) alarmButton.isOn = false;
    }

    public void sendNotification(Notifications new_notif)
    {
        //Debug.Log("new notification : " + new_notif.ActualTime.ToString()+" ; "+new_notif.Prefab_key.ToString());
        notificationList.Add(new_notif); //ajoute la notification principale
        notif_num++;
        updateNotif();
    }

    private void dequeNewTips(int value)
    {
        //Debug.Log("in deque new tips at value : "+value+" Total : "+notificationList.Count);
        actual_notif = notificationList[value];
        notificationList.RemoveAt(value);

        notif_num--;
        updateNotif();

        RectTransform temp_rectTranfrom = actual_go.GetComponentInChildren<LayoutGroup>().GetComponent<RectTransform>();

        for (int i = 0; i < prefab_list.Count; i++)
        {
            if (string.Compare(prefab_list[i].name, actual_notif.Prefab_key) == 0)
            {
                actual_go.SetActive(false);
                Destroy(actual_go);

                actual_go = (GameObject)Instantiate(prefab_list[i], transform_parent);
                actual_go.GetComponentInChildren<Button>().onClick.AddListener(delegate { closeActualPopup(false); });
                actual_go.GetComponentInChildren<Button>().onClick.AddListener(showTipsList);

                time_diff = DateTime.Now - actual_notif.ActualTime;
                if (time_diff.Days > 1)
                {
                    time_string = time_diff.Days.ToString("###") + " days ago";
                }
                else if (time_diff.Hours > 1)
                {
                    time_string = time_diff.Hours.ToString("###") + " hours ago";
                }
                else if (time_diff.Minutes > 1)
                {
                    time_string = time_diff.Minutes.ToString("###") + " minutes ago";
                }
                else
                {
                    time_string = time_diff.Seconds.ToString("###") + " seconds ago";
                }

                actual_go.GetComponentInChildren<Text>().text = time_string;
            }
        }
    }

    private void discardElement(int value)
    {
        //Debug.Log("in discard element() at value : "+value + " Total : " + notificationList.Count);
        notificationList.RemoveAt(value);
        notif_num--;
        updateNotif();
        actual_go.SetActive(false);
        Destroy(actual_go);
        if (!(notif_num <= 0))
        {
            showTipsList();
        }
        else
        {
            notif_num = 0;
            actual_go = (GameObject)Instantiate(default_tips, transform_parent);
            actual_go.GetComponentInChildren<Button>().onClick.AddListener(delegate { closeActualPopup(true); });
        }
    }

    private void showTipsList()
    {
        if (!(notif_num <= 0))
        {
            actual_go = (GameObject)Instantiate(List_tips, transform_parent);
            actual_go.GetComponentInChildren<Button>().onClick.AddListener(delegate { closeActualPopup(false); });

            RectTransform temp_rectTranfrom = actual_go.GetComponentInChildren<LayoutGroup>().GetComponent<RectTransform>();

            for (int j = 0; j < notificationList.Count; j++)
            {
                for (int i = 0; i < prefab_list.Count; i++)
                {
                    if (string.Compare(prefab_list[i].name, notificationList[j].Prefab_key) == 0)
                    {
                        temp_rectTranfrom.sizeDelta += new Vector2(0, Space_change);
                        GameObject tempobj = (GameObject)Instantiate(prefab_mini_list[i], temp_rectTranfrom);
                        Button[] temp_list = tempobj.GetComponentsInChildren<Button>();
                        int temp = j;
                        temp_list[0].onClick.AddListener(delegate { dequeNewTips(temp); }); //delegate{SomeMethodName(SomeObject);}
                        temp_list[1].onClick.AddListener(delegate { discardElement(temp); });

                        time_diff = DateTime.Now - notificationList[j].ActualTime;
                        if (time_diff.Days > 1)
                        {
                            time_string = time_diff.Days.ToString("###") + " days ago";
                        }
                        else if (time_diff.Hours > 1)
                        {
                            time_string = time_diff.Hours.ToString("###") + " hours ago";
                        }
                        else if (time_diff.Minutes > 1)
                        {
                            time_string = time_diff.Minutes.ToString("###") + " minutes ago";
                        }
                        else
                        {
                            time_string = time_diff.Seconds.ToString("###") + " seconds ago";
                        }

                        actual_go.GetComponentInChildren<Text>().text = notif_num.ToString("###");
                        tempobj.GetComponentInChildren<Text>().text = time_string;
                    }
                }
            }
        }
        else
        {
            notif_num = 0;
            actual_go = (GameObject)Instantiate(default_tips, transform_parent);
            actual_go.GetComponentInChildren<Button>().onClick.AddListener(delegate { closeActualPopup(true); });
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