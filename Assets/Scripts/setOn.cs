using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setOn : MonoBehaviour
{ 
    public Toggle toggleButton;
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
            }
            else
            {
                // Debug.Log("i have already a old value");
                lightSlider.value = oldValue;
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
            }
        }
    }

    public void onChange(float value)
    {
        oldValue = value;
    }
}
