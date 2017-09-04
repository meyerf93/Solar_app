using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptTime : MonoBehaviour {

    public Slider slide;

    public double max_voltage;
    public double max_kwh;
    public double current_divider;

    public Text text_to_change;
    public void Start()
    {
        double instant_power = ((slide.value / current_divider) * max_voltage) / 1000;
        //Debug.Log("instante_power : " + instant_power);
        double time_hours = max_kwh / instant_power;
        text_to_change.text = "~" + time_hours.ToString("F1") + " h";
    }

    public void OnChange(float value)
    {
        
        double instant_power = ((value / current_divider) * max_voltage)/1000;
        //Debug.Log("instante_power : " + instant_power);
        double time_hours = max_kwh / instant_power;
        text_to_change.text = "~" + time_hours.ToString("F1")+" h";
    }
}
