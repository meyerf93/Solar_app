using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HG.iot.mqtt.example;

public class setOn : MonoBehaviour
{ 
    public Toggle toggleButton;
    public LightReceivers command;
    private Slider lightSlider;
    private float oldValue;

    public void onClick(bool status)
    {
        lightSlider = toggleButton.GetComponentInChildren<Slider>();
        //Debug.Log("debug on click for light : " + oldValue + " ; " + lightSlider.value);
        if (status)
        {
            if (oldValue == 0)
            {
                lightSlider.value = 100;
                command.OnChange(100);
            }
            else
            {
                // Debug.Log("i have already a old value");
                lightSlider.value = oldValue;
                command.OnChange(oldValue);
            }
        }
        else
        {
            if (toggleButton.group.AnyTogglesOn())
            {
                //Debug.Log("Another toogle is on");
                oldValue = lightSlider.value;
            }
            else
            {
                oldValue = lightSlider.value;
                //Debug.Log("no other toogle is on : " + oldValue);
                lightSlider.value = 0;
                command.OnChange(0);
            }
        }
    }

    public void onChange(float value)
    {
        oldValue = value;
    }

    public void sendValue(float value) {
        command.OnChange(value);
    }

    public void BlockSlider(bool value)
    {
        command.BlockSlider(value);
    }
}
