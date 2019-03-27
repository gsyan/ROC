using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterInfo
{
    public AttackInfo attackInfo;
    public DefenceInfo defenceInfo;
    public MoveInfo moveInfo;

    public string name;
    public int hpCur;
    public int hpMax;

    public virtual void SetInfo()
    {
        attackInfo = new AttackInfo();
        defenceInfo = new DefenceInfo();
        moveInfo = new MoveInfo();

        name = "";
        hpCur = 0;
        hpMax = 0;
    }

    public virtual void SetInfo(CharacterInfo ci)
    {
        DeepCopy(ci);
    }
    public virtual void DeepCopy(CharacterInfo ci)
    {
        attackInfo.DeepCopy(ci.attackInfo);
        defenceInfo.DeepCopy(ci.defenceInfo);
        moveInfo.DeepCopy(ci.moveInfo);

        name = ci.name;
        hpCur = ci.hpCur;
        hpMax = ci.hpMax;

    }



}
