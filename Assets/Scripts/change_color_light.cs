using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class change_color_light : MonoBehaviour {
	public Toggle Button_light_quality;
    public Toggle Button_light_types;

    public Text text_quality;
    public Text text_types;

    public GameObject PopUp_quality;
    public GameObject PopUp_types;

    private Color start_color;

    private void Start()
    {
        start_color = text_quality.color;
    }

    public void change_quallity_color(){
		if (Button_light_quality.isOn == true) {
			text_quality.color = Color.white;
		}
		else if(PopUp_quality.activeSelf == false){
            text_quality.color = start_color;
		}
	}

    public void change_types_color()
    {
        if (Button_light_types.isOn == true)
        {
            text_types.color = Color.white;
        }
        else if(PopUp_types.activeSelf == false)
        {
            text_types.color = start_color;
        }
    }
}