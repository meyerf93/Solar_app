using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour {

	public void onPurchase(Condition conditionPurchased)
	{
		conditionPurchased.satisfied = GetComponent<Toggle>().isOn;
	}
}
