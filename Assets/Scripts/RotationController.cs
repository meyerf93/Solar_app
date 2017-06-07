using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationController : MonoBehaviour {

    public SimpleTouchPad touchPad;
    public Slider roatateSlier;
    public GameObject Building;
    public int multiplier;
    public Vector3 rotation;

	
	// Update is called once per frame
	void Update () {
		if(touchPad.GetDirection() != Vector2.zero)
        {
            Building.transform.rotation = Quaternion.AngleAxis(multiplier * touchPad.GetDirection().x, rotation);
        }
	}
}
