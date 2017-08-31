using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getTime : MonoBehaviour {

    public Text time_text;
	
	// Update is called once per frame
	void Update () {
        time_text.text = System.DateTime.Now.ToString("hh:mm tt");
	}
}
