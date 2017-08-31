using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradiantManager : MonoBehaviour {

    public Image TopGradiant;
    public Image BotGradiant;

    public float TopThreshold;
    public float BotThreshold;

    public void onChange(Vector2 value)
    {
       //Debug.Log("value of vector with onChange : " + value);
       if(value.y == TopThreshold)
        {
            TopGradiant.enabled = false;
            BotGradiant.enabled = true;
        }
       else if (value.y == BotThreshold)
        {
            TopGradiant.enabled = true;
            BotGradiant.enabled = false;
        }
        else
        {
            TopGradiant.enabled = true;
            BotGradiant.enabled = true;
        }
    }
}
