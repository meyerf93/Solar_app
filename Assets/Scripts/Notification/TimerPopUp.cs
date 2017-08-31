using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerPopUp : MonoBehaviour {

    public float timeLeft;
    public Toggle alarmButton;
    public Image image;

    private float timer;
    private void Start()
    {
        timer = timeLeft;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = timeLeft;
            image.gameObject.SetActive(true);
        }
    }

    void ClickAlarm()
    {
        image.gameObject.SetActive(false);
        timer = timeLeft;
        alarmButton.isOn = false;
    }
}
