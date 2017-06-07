using UnityEngine;
using UnityEngine.UI;

public class RotationReaction: DelayedReaction
{
    public GameObject Building;
    public Slider Sliderval;
    public float defaultRotation;
    public Vector3 rotation;
    public int multiplier;


    private float Slidervalfloat;

    protected override void SpecificInit()
    {
        Slidervalfloat = Sliderval.value;
        Building.transform.rotation = Quaternion.AngleAxis(multiplier*defaultRotation, rotation);
    }


    protected override void ImmediateReaction ()
    {
         Slidervalfloat = Sliderval.value;
         Building.transform.rotation = Quaternion.AngleAxis(multiplier * Slidervalfloat, rotation);

    }
}
