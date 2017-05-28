using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Building : ScriptableObject {

    public GameObject Roof;
    public GameObject Ground;

    public GameObject West_facade;
    public GameObject East_facade;
    public GameObject South_facade;
    public GameObject North_facades;

    public GameObject West_skin;
    public GameObject East_skin;
    public GameObject South_skin;
    public GameObject North_skin;

    public GameObject West_core;
    public GameObject East_core;
    public GameObject South_core;
    public GameObject North_core;
}
