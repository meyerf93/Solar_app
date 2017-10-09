using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset_popup : MonoBehaviour
{

    public Toggle[] pop_up_list;

    // Use this for initialization
    public void ResetPopup()
    {
        for (int i = 0;i < pop_up_list.Length; i++)
        {
            pop_up_list[i].isOn = false;
        }
    }
}
