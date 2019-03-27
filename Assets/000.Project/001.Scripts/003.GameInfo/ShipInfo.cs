using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShipInfo : CharacterInfo
{
    public ShipType type;
    public int grade;
    public int linkCur;
    public int linkMax;
    public int belongFleet;                     //-1 이면 소속 없음
    public bool bLockFromDisassemble;           //분해 불가 락


    /// <summary>
    /// 초기화 수준의 생성
    /// </summary>
    public override void SetInfo()
    {
        base.SetInfo();

        type = ShipType.None;
        grade = 0;
        linkCur = 0;
        linkMax = 0;
        belongFleet = -1;
        bLockFromDisassemble = false;
    }
    
    public override void SetInfo(CharacterInfo ci)
    {
        base.SetInfo(ci);

        ShipInfo si = (ShipInfo)ci;
        type = si.type;
        grade = si.grade;
        linkCur = si.linkCur;
        linkMax = si.linkMax;
        belongFleet = si.belongFleet;
        bLockFromDisassemble = si.bLockFromDisassemble;

    }

}


