using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Change_color_car : MonoBehaviour {

    public Toggle Button_Consult_state;
    public Toggle Button_Modify;
    public Toggle Button_parameters;

    public Text text_state;
    public Text text_modify;
    public Text text_parameters;

    public GameObject PopUp_state;
    public GameObject PopUp_modifiy;
    public GameObject PopUp_parameters;

    private Color start_color;

    private void Start()
    {
        start_color = text_state.color;
    }

    public void change_state_color()
    {
        if (Button_Consult_state.isOn == true)
        {
            text_state.color = Color.white;
        }
        else if (PopUp_state.activeSelf == false)
        {
            text_state.color = start_color;
        }
    }

    public void change_modify_color()
    {
        if (Button_Modify.isOn == true)
        {
            text_modify.color = Color.white;
        }
        else if (PopUp_modifiy.activeSelf == false)
        {
            text_modify.color = start_color;
        }
    }

    public void change_parameters_color()
    {
        if (Button_parameters.isOn == true)
        {
            text_parameters.color = Color.white;
        }
        else if (PopUp_parameters.activeSelf == false)
        {
            text_parameters.color = start_color;
        }
    }
}
