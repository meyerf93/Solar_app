using UnityEngine;
using UnityEngine.UI;
public class SliderReaction : Reaction
{
    public float value;
    public string Signe;
    public Slider Add_Slider;
    public Slider base_Slider;

    protected override void SpecificInit()
    {
        base_Slider = FindObjectOfType<Slider>();
        Add_Slider = FindObjectOfType<Slider>();
    }
    protected override void ImmediateReaction()
    {
        base_Slider.value = 1.2F;
    }
}