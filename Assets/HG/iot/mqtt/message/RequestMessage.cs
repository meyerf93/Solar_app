using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HG.iot.mqtt.example
{
    [Serializable]
    public class RequestMessage : Message
    {
        public string cmd;
        public string mdl;

        //{"cmd":"knx1/:1.1.26/:/dim.1","mdl":"knx1"}
    }
}

