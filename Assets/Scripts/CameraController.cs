using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speedRotation;
	public GameObject Model_3D;
	public Slider Sliderval;

	private float Slidervalfloat;

	void FixedUpdate(){
		Slidervalfloat = Sliderval.value;
		//print("value as changed : " + Slidervalfloat);

		Model_3D.transform.rotation = Quaternion.AngleAxis (Slidervalfloat, Vector3.up);
		//Vector3 movement = new Vector3(0.0f, Slidervalfloat, 0.0f);

		//Model_3D.transform.Rotate(movement * speedRotation * Time.deltaTime);
	}
}
