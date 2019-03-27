using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AttackInfo
{
    public BeamType beamType;
    public int beamGunCount;
    public float beamCool;

    public MissleType missleType;
    public int missleGunCount;
    public float missleCool;

    public FighterType fighterType;
    public int fighterCountCur;
    public int fighterCountMax;
    public float fighterCool;

    public float attackRange;                 // 공격거리


    public AttackInfo()
    {
        beamType = BeamType.None;
        beamGunCount = 0;
        beamCool = 0.0f;

        missleType = MissleType.None;
        missleGunCount = 0;
        missleCool = 0.0f;

        fighterType = FighterType.None;
        fighterCountCur = 0;
        fighterCountMax = 0;
        fighterCool = 0.0f;

        attackRange = 0.0f;
    }

    public AttackInfo(AttackInfo ai)
    {
        DeepCopy(ai);
    }

    public void DeepCopy(AttackInfo ai)
    {
        beamType = ai.beamType;
        beamGunCount = ai.beamGunCount;
        beamCool = ai.beamCool;

        missleType = ai.missleType;
        missleGunCount = ai.missleGunCount;
        missleCool = ai.missleCool;

        fighterType = ai.fighterType;
        fighterCountCur = ai.fighterCountCur;
        fighterCountMax = ai.fighterCountMax;
        fighterCool = ai.fighterCool;

        attackRange = ai.attackRange;
    }

}
