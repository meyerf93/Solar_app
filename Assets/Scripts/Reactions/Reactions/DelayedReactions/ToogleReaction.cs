using UnityEngine;
using UnityEngine.UI;

public class ToogleReaction: DelayedReaction
{
    public GameObject gameObject;
    public Toggle toogleStatus;


    protected override void ImmediateReaction()
    {
        bool temp_value = toogleStatus.isOn;
        gameObject.SetActive ( temp_value);
    }
}