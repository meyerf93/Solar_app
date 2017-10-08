using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HG.iot.mqtt.example
{
    [Serializable]
    public class CarMessage : Message
    {
        public string data;
        public string t;
        public string id;

        //{"dttp": null, "data": "is conencted", "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
    }
}
