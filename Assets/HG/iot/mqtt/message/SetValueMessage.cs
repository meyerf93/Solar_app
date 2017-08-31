using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace HG.iot.mqtt.example
{
    [Serializable]
    public class SetValueMessage : Message
    {
        public string cmd;
        public string mdl;
        public double value;
        //{"cmd":"knx1/:1.1.13/:/power.1","mdl":"knx1","value":10}
    }
}


