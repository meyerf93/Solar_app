﻿using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI;         public class ShowPopUp : MonoBehaviour {      public GameObject PopUp;     public int numberOfClick;     public Toggle Button;     public Image backgroundText;      private int CountClick;  	// Use this for initialization 	void Start () {         CountClick = 0; 	}      public void ClickPopUp(int addClick){         CountClick = CountClick + 1;         if(CountClick == numberOfClick){             PopUp.SetActive(true);             Button.isOn = true;             backgroundText.gameObject.SetActive(false);         }     } 	 public void ClosePopUp()     {         CountClick = 0;         PopUp.SetActive(false);         Button.isOn = false;         backgroundText.gameObject.SetActive(false);     } } 