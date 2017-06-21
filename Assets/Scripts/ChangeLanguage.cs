using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class ChangeLanguage : MonoBehaviour {

    public Text car_pop_up;
    public Text car_counter;
    public Text total_consum;
    public Text state_charger;
    public Text status_title;
    public Text production;
    public Text consumption;

    private string old_lang = "English";

    private Lang LMan;
    private string currentLang = "English";
    private void Start()
    {
        LMan = new Lang("D:\\WORK\\work unity\\SLC_Interface\\Assets\\resources\\Language.xml", currentLang, false);
        old_lang = "English";
    }

    public void change_language()
    {
        if (old_lang.Equals("English")){
            LMan.setLanguage("D:\\WORK\\work unity\\SLC_Interface\\Assets\\resources\\Language.xml", "French");
            old_lang = "French";
        }
        else
        {
            LMan.setLanguage("D:\\WORK\\work unity\\SLC_Interface\\Assets\\resources\\Language.xml", "English");
            old_lang = "English";
        }

        car_pop_up.text = LMan.getString("car_pop_up");
        car_counter.text = LMan.getString("car_counter");
        total_consum.text = LMan.getString("total_consum");
        state_charger.text = LMan.getString("state_charger");
        status_title.text = LMan.getString("status_title");
        production.text = LMan.getString("production");
        consumption.text = LMan.getString("consumption");
    }

}
