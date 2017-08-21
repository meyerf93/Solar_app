using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentChange : MonoBehaviour {
    public Slider actual_slider;
    public Text text_value;
    public Slider max_value_slider;

    public void Onchange(float value)
    {
        text_value.text = (value / max_value_slider.value * 100).ToString("F1")+" %";
    }

    public void MaxSliderChange(float value)
    {
        text_value.text = (actual_slider.value / value * 100).ToString("F1") + " %";
    }
}
