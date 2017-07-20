using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLightBackground : MonoBehaviour {

    public Toggle toggleButton;
    public Image newBackground;

    private Slider lightSlider;

    public void changeBacground()
    {
        lightSlider = toggleButton.GetComponentInChildren<Slider>();
        if(lightSlider.value > 0)
        {
            newBackground.gameObject.SetActive(true);
            newBackground.enabled = true;
        }
        else
        {
            newBackground.gameObject.SetActive(false);
            newBackground.enabled = false;
        }
    }

}
