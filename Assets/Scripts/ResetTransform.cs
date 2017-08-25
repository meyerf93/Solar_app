using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransform : MonoBehaviour {

    public Transform transf;

    public void Reset_transform()
    {
        transf.rotation = new Quaternion();
    }
}
