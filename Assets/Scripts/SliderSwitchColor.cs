using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSwitchColor : MonoBehaviour {
    public Text text_value;

    public Sprite low;
    public Sprite mid;
    public Sprite high;

    public string Unit;

    public Slider slide;
    public Image fill;

    public int lowThress;
    public int midThress;

    public void OnChange(float value)
    {
        //Debug.Log("value of slider : " + slide.value + " ; thresshold : " + slide.maxValue / 100 * midThress);
        if(slide.value > slide.maxValue/100*midThress)
        {
            fill.sprite = high;
        }
        else if(slide.value > slide.maxValue / 100 * lowThress)
        {
            fill.sprite = mid;
        }
        else
        {
            fill.sprite = low;
        }
        text_value.text = value.ToString("F2") + " "+Unit;
    }

}
