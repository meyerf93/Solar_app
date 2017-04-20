using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toogle_visibility : MonoBehaviour {

	public GameObject Visible;
	public GameObject Invisible;

	public Toggle Button;

	public	void toogle_visible(){
		if (Button.isOn == true) {
			Invisible.SetActive (false);
			Visible.SetActive (true);
		}
		else {
			Visible.SetActive (false);
			Invisible.SetActive (true);
		}
	}
}
