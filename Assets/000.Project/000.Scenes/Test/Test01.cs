using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test01 : MonoBehaviour {

    public Transform obj1;
    public Transform obj2;



    void Update ()
    {
        float dot = Vector3.Dot(obj1.forward, obj2.forward);
        float angle = 0.0f;
        MathBK.AngleFromDot(dot, ref angle);
        Debug.Log(angle);


    }
}
