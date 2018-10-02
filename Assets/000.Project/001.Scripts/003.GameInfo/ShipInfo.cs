using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfo
{
    public BeamType beamType;
    public int beamGunCount;
    public int beamCool;

    public MissleType missleType;
    public int missleGunCount;
    public int missleCool;

    public FighterType fighterType;
    public int fighterCountCur;
    public int fighterCountMax;
    public int fighterCool;

    public ShieldType shieldType;

    public int defence;

    public int hpCur;
    public int hpMax;

    public int moveSpeed;
    public int rotateSpeed;
    public int attackRange;                 // 공격거리

    public void DeepCopy(BattleInfo bi)
    {
        beamType = bi.beamType;
        beamGunCount = bi.beamGunCount;
        beamCool = bi.beamCool;

        missleType = bi.missleType;
        missleGunCount = bi.missleGunCount;
        missleCool = bi.missleCool;

        fighterType = bi.fighterType;
        fighterCountCur = bi.fighterCountCur;
        fighterCountMax = bi.fighterCountMax;
        fighterCool = bi.fighterCool;

        shieldType = bi.shieldType;

        defence = bi.defence;

        hpCur = bi.hpCur;
        hpMax = bi.hpMax;

        moveSpeed = bi.moveSpeed;
        rotateSpeed = bi.rotateSpeed;
        attackRange = bi.attackRange;
    }
}


public class ShipInfo
{
    public BattleInfo battleInfo;

    public string name;
    public ShipType type;
    public int grade;
    public int linkCur;
    public int linkMax;
    public int belongFleet;                     //-1 이면 소속 없음
    public bool bLockFromDisassemble;           //분해 불가 락

    public void SetBattleInfo(BattleInfo bi)
    {
        battleInfo = bi;
    }
    public void SetInfo(ShipInfo si)
    {
        SetBattleInfo(si.battleInfo);

        name = si.name;
        type = si.type;
        grade = si.grade;
        linkCur = si.linkCur;
        linkMax = si.linkMax;
        belongFleet = si.belongFleet;
        bLockFromDisassemble = si.bLockFromDisassemble;
    }

    public void DeepCopyBattleInfo(BattleInfo bi)
    {
        battleInfo = new BattleInfo();
        battleInfo.DeepCopy(bi);
    }
    public void DeepCopyInfo(ShipInfo si)
    {
        DeepCopyBattleInfo(si.battleInfo);

        name = si.name;
        type = si.type;
        grade = si.grade;
        linkCur = si.linkCur;
        linkMax = si.linkMax;
        belongFleet = si.belongFleet;
        bLockFromDisassemble = si.bLockFromDisassemble;
    }


}


