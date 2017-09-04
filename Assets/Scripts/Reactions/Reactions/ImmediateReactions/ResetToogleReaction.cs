using UnityEngine;
using UnityEngine.UI;

public class ResetToogleReaction : Reaction {

    public Toggle toggle_button;
    public bool default_value;
    public float delay;

    // Update is called once per frame
    protected override void ImmediateReaction()
    {
        toggle_button.isOn = default_value;
    }

}
