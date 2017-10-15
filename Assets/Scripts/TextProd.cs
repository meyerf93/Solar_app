using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextProd : MonoBehaviour {

    public Slider all_prod;

    public Text text_1;
    public Text text_2;
    public Text text_3;
    public Text text_4;
    public Text Default;

    public double thresshold_1;
    public double thresshold_2;
    public double thresshold_3;
    public double thresshold_4;

    public void OnChange(float value)
    { 
        if (value > thresshold_4)
        {
            text_1.enabled = false;
            text_2.enabled = false;
            text_3.enabled = false;
            text_4.enabled = true;
            Default.enabled = false;
        }
        else if (value > thresshold_3)
        {
            text_1.enabled = false;
            text_2.enabled = false;
            text_3.enabled = true;
            text_4.enabled = false;
            Default.enabled = false;
        }
        else if(value > thresshold_2)
        {
            text_1.enabled = false;
            text_2.enabled = true;
            text_3.enabled = false;
            text_4.enabled = false;
            Default.enabled = false;
        }
        else if (value > thresshold_1)
        {
            text_1.enabled = true;
            text_2.enabled = false;
            text_3.enabled = false;
            text_4.enabled = false;
            Default.enabled = false;
        }
        else
        {
            text_1.enabled = false;
            text_2.enabled = false;
            text_3.enabled = false;
            text_4.enabled = false;
            Default.enabled = true;
        }
    }
}