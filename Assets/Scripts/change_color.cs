using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change_color : MonoBehaviour {
	public Toggle Button;
	public Text item;

	public void change_text_color_color(){
		if (Button.isOn == true) {
			item.color = Color.white;
		}
		else {
			item.color = new Color (1, 1, 1);
		}
	}
}
