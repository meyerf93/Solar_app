using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class home_buildings : MonoBehaviour {

    public Building building_saved_data;
    public float value_of_rotation;

    private GameObject building;
    private GameObject Roof;
    private GameObject Ground;

    private GameObject West_facade;
    private GameObject East_facade;
    private GameObject South_facade;
    private GameObject North_facades;

    private GameObject West_skin;
    private GameObject East_skin;
    private GameObject South_skin;
    private GameObject North_skin;

    private GameObject West_core;
    private GameObject East_core;
    private GameObject South_core;
    private GameObject North_core;

   
    
	// Use this for initialization
	void Start () {
        building = new GameObject("building_base");

        Roof = new GameObject("Roof");
        Roof.SetActive(true);
        Roof.transform.parent = building.transform;
        Roof = building_saved_data.Roof;
        Ground = new GameObject("Gground");
        Ground.SetActive(true);
        Ground.transform.parent = building.transform;

        West_facade = new GameObject("West_facade");
        West_facade.SetActive(true);
        West_facade.transform.parent = building.transform;
        East_facade = new GameObject("East_facade");
        East_facade.SetActive(true);
        East_facade.transform.parent = building.transform;
        South_facade = new GameObject("South_facade");
        South_facade.SetActive(true);
        South_facade.transform.parent = building.transform;
        North_facades = new GameObject("Norht_facade");
        North_facades.SetActive(true);
        North_facades.transform.parent = building.transform;

        West_skin = new GameObject("West_skin");
        West_skin.SetActive(true);
        West_skin.transform.parent = building.transform;
        East_skin = new GameObject("East_skin");
        East_skin.SetActive(true);
        East_skin.transform.parent = building.transform;
        South_skin = new GameObject("South_skin");
        South_skin.SetActive(true);
        South_skin.transform.parent = building.transform;
        North_skin = new GameObject("Norht_skin");
        North_skin.SetActive(true);
        North_skin.transform.parent = building.transform;

        West_core = new GameObject("West_core");
        West_core.SetActive(true);
        West_core.transform.parent = building.transform;
        East_core = new GameObject("East_core");
        East_core.SetActive(true);
        East_core.transform.parent = building.transform;
        South_core = new GameObject("South_core");
        South_core.SetActive(true);
        South_core.transform.parent = building.transform;
        North_core = new GameObject("Norht_core");
        North_core.SetActive(true);
        North_core.transform.parent = building.transform;


        building.transform.rotation = Quaternion.AngleAxis(value_of_rotation, Vector3.up);
    }
}
