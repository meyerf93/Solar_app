using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications {

    private DateTime actualTime;
    private string prefab_key;

    public Notifications (string key)
    {
        actualTime = DateTime.Now;
        prefab_key = key;
    }

    public string Prefab_key
    {
        get
        {
            return prefab_key;
        }

        set
        {
            prefab_key = value;
        }
    }

    public DateTime ActualTime
    {
        get
        {
            return actualTime;
        }

        set
        {
            actualTime = value;
        }
    }
}
