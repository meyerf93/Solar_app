using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSliderToogle : MonoBehaviour {
    public GameObject toggleToModifyParent;
    public Slider mainSlider;

    private Toggle[] tempToggleList;
    private Slider tempSlider;
    private setOn tempSetOn;

    public void Start()
    {
        tempToggleList = toggleToModifyParent.GetComponentsInChildren<Toggle>();
    }
    public void onChange(float value)
    {
        for(int i= 0; i < tempToggleList.Length; i++)
        {
            tempSlider = tempToggleList[i].GetComponentInChildren<Slider>();
            tempSetOn = tempToggleList[i].GetComponent<setOn>();
            if (tempToggleList[i].isOn)
            {
                tempSlider.value = value;
                tempSetOn.sendValue(tempSlider.value);
            }
        }
    }

    public void BlockSlider(bool value)
    {
        for (int i = 0; i < tempToggleList.Length; i++)
        {
            tempSetOn = tempToggleList[i].GetComponent<setOn>();
            tempSetOn.BlockSlider(value);
        }
    }
}
