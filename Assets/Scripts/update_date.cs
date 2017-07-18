using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class update_date : MonoBehaviour {

    public Text date_text;
    // Update is called once per frame
	void Update () {
        date_text.text = System.DateTime.Now.ToString("ddd. dd MMMM yyyy");
    }
}
