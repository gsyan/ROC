using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MoveInfo
{
    public float moveSpeed;
    public float rotateSpeed;


    public MoveInfo()
    {
        moveSpeed = 0.0f;
        rotateSpeed = 0.0f;
    }

    public MoveInfo(MoveInfo mi)
    {
        DeepCopy(mi);
    }

    public void DeepCopy(MoveInfo mi)
    {
        moveSpeed = mi.moveSpeed;
        rotateSpeed = mi.rotateSpeed;
    }


}
