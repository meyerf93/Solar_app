using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HG.iot.mqtt.example
{
	[Serializable]
	public sealed class GlobalMessage: Message
	{
		public string dttp;
		public double data;
        public string t;
        public string id;

        //{"dttp": null, "data": 100, "t": "2017-05-15T06:47:42Z", "id": "knx1/:1.2.26/:/dim.7"}
    }
}