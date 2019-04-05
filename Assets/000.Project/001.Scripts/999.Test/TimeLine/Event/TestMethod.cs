using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Light l = GetComponent<Light>();
    }

    public void TurnOff()
    {
        Light l = GetComponent<Light>();
        l.intensity = 0.0f;
    }


}
