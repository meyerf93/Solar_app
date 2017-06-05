using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSliderToogle : MonoBehaviour {
    public GameObject toggleToModifyParent;
    public Slider mainSlider;

    private Toggle[] tempToggleList;
    private Slider tempSlider;

    public void Start()
    {
        tempToggleList = toggleToModifyParent.GetComponentsInChildren<Toggle>();
    }
    public void onChange()
    {
        for(int i= 0; i < tempToggleList.Length; i++)
        {
            tempSlider = tempToggleList[i].GetComponentInChildren<Slider>();
            if (tempToggleList[i].isOn)
            {
                tempSlider.value = mainSlider.value;
            }
        }
    }
}
