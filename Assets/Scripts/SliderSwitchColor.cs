using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSwitchColor : MonoBehaviour {
    public Text text_value;
    public string Unit;

    public string virgule_number;
    public double divider;

    public void OnChange(float value)
    {
        //Debug.Log("value of slider : " + slide.value + " ; thresshold : " + slide.maxValue / 100 * midThress);
        if(virgule_number == ""){
            text_value.text = (value / divider).ToString("F2") + " " + Unit;
        }
        else
        {
            text_value.text = (value/divider).ToString(virgule_number) + " " + Unit;
        }
    }

}
