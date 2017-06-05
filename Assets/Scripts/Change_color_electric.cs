using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Change_color_electric : MonoBehaviour {

    public Toggle Button_electric_activity;
    public Toggle Button_electric_season;
    public Toggle Button_electric_sun;
    public Toggle Button_electric_storage;

    public Text text_activity;
    public Text text_season;
    public Text text_sun;
    public Text texct_storage;

    public GameObject PopUp_activity;
    public GameObject PopUp_seasons;
    public GameObject PopUp_suns_path;
    public GameObject PopUp_storage;

    private Color start_color;

    private void Start()
    {
        start_color = text_activity.color;
    }

    public void change_activity_color()
    {
        if (Button_electric_activity.isOn == true)
        {
            text_activity.color = Color.white;
        }
        else if (PopUp_activity.activeSelf == false)
        {
            text_activity.color = start_color;
        }
    }

    public void change_season_color()
    {
        if (Button_electric_season.isOn == true)
        {
            text_season.color = Color.white;
        }
        else if (PopUp_seasons.activeSelf == false)
        {
            text_season.color = start_color;
        }
    }

    public void change_sun_color()
    {
        if (Button_electric_sun.isOn == true)
        {
            text_sun.color = Color.white;
        }
        else if (PopUp_suns_path.activeSelf == false)
        {
            text_sun.color = start_color;
        }
    }

    public void change_storage_color()
    {
        if (Button_electric_storage.isOn == true)
        {
            texct_storage.color = Color.white;
        }
        else if (PopUp_storage.activeSelf == false)
        {
            texct_storage.color = start_color;
        }
    }
}
