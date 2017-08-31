using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HG.iot.mqtt.example
{
    [Serializable]
    public sealed class ProductionJson : Message
    {
        public string cmd;
        public string mdl;
        public double value;
    }
}

//json format for production
