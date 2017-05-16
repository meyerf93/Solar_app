using UnityEngine;
using System.Collections;

namespace HG.webgl
{
	public class UnityJavascriptInterop : MonoBehaviour 
	{
		void Start () 
		{
			// let javascript know that Unity is ready
			// this script should be the last script in execution order
			#if UNITY_WEBGL && !UNITY_EDITOR
			HGUNITY3DJS.Ready();
			#endif
		}
	}
}
