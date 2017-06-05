using UnityEngine;
using UnityEngine.UI;

public class SimpleRotationReaction: DelayedReaction
{
    public GameObject Building;
    public float defaultRotation;
    public Vector3 rotation;


    protected override void ImmediateReaction ()
    {
         Building.transform.rotation = Quaternion.AngleAxis(defaultRotation, rotation);

    }
}
