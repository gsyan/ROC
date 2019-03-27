using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DefenceInfo
{
    public ShieldType shieldType;

    public int defence;


    public DefenceInfo()
    {
        shieldType = ShieldType.None;
        defence = 0;
    }

    public DefenceInfo(DefenceInfo di)
    {
        DeepCopy(di);
    }

    public void DeepCopy(DefenceInfo di)
    {
        shieldType = di.shieldType;

        defence = di.defence;
    }

}
