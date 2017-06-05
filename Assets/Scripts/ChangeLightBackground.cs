using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLightBackground : MonoBehaviour {

    public Toggle toggleButton;
    public Image newBackground;
    public Image oldBackground;

    private Slider lightSlider;

    public void changeBacground()
    {
        lightSlider = toggleButton.GetComponentInChildren<Slider>();
        if(lightSlider.value > 0)
        {
            newBackground.gameObject.SetActive(true);
            newBackground.enabled = true;
            oldBackground.gameObject.SetActive(false);
            oldBackground.enabled = false;
            toggleButton.targetGraphic = newBackground;
        }
        else
        {
            newBackground.gameObject.SetActive(false);
            newBackground.enabled = false;
            oldBackground.gameObject.SetActive(true);
            oldBackground.enabled = true;
            toggleButton.targetGraphic = oldBackground;
        }
    }

}
