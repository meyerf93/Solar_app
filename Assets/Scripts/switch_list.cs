using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class switch_list : MonoBehaviour {

	public GameObject Idle_list;
	public GameObject Light_list;
	public GameObject Electric_list;
	public GameObject Car_list;


	public void switch_home(){
		Idle_list.SetActive (true);
		Light_list.SetActive (false);
		Electric_list.SetActive (false);
		Car_list.SetActive (false);
	}

	public void switch_light(){
		Light_list.SetActive (true);
		Idle_list.SetActive (false);
		Electric_list.SetActive (false);
		Car_list.SetActive (false);
	}

	public void switch_electric(){
		Electric_list.SetActive (true);
		Idle_list.SetActive (false);
		Light_list.SetActive (false);
		Car_list.SetActive (false);
	}

	public void switch_car(){
		Car_list.SetActive (true);
		Idle_list.SetActive (false);
		Light_list.SetActive (false);
		Electric_list.SetActive (false);
	}
}
