using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 이걸 해주지 않으면 serializedObject.FindProperty("practiceArray"); 에서 ,Null 이 나옴
public class Practice01Sub
{
    public string name;
    public float value;
}

public class Practice01 : MonoBehaviour
{
    public Vector3 lookAtPosition = Vector3.zero;
    public int intValue;

    public Practice01Sub[] practiceArray;


    public void Update()
    {
        
    }
}
