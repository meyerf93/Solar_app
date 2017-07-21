using UnityEngine;
using UnityEngine.UI;

public class RotationReaction: DelayedReaction
{
    public GameObject Building;
    public SimpleRotation rotation;
    public int multiplier;
    public Vector3 axis;

    protected override void SpecificInit()
    {
        float intensity = rotation.GetDirection().x;
        //Debug.Log("Vector for the rotation : " + intensity);

        Building.transform.Rotate(axis, intensity * multiplier*Mathf.Deg2Rad);
    }


    protected override void ImmediateReaction ()
    {
        float intensity = rotation.GetDirection().x;
        //Debug.Log("Vector for the rotation : " + intensity);

        Building.transform.Rotate(axis, intensity * multiplier*Mathf.Deg2Rad);

    }
}
