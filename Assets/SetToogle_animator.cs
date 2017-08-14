using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetToogle_animator : MonoBehaviour {

    public Animator anim;
    public Toggle button;
    public string bool_name;

    void onValueChanged(bool value)
    {
        anim.SetBool(bool_name, button.IsActive());
    }
}
