using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//json format for production
//{"dttp": null, "data": 42.92, "t": "2017-05-21T08:49:28Z", "id": "knx1/:1.1.34/:/temp.1"}

[Serializable]
public class ProductionJson{
    public double data;
    public string t;
    public string id;
}

