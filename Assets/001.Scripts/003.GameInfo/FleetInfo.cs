using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetInfo
{
    public BattleInfo battleInfo = new BattleInfo();

    public List<ShipInfo> shipInfoList;

    public FleetFormationType formationType;

    

    public void Set(FleetInfo fleetInfo)
    {
        battleInfo = fleetInfo.battleInfo;
        shipInfoList = fleetInfo.shipInfoList;
        formationType = fleetInfo.formationType;
        
    }
	
    public void SetShipInfoList(List<ShipInfo> inShipInfoList)
    {
        shipInfoList = inShipInfoList;
        UpdateBattleInfo();
    }
    private void SetAbilityZero()
    {
        battleInfo.beamGunCount = 0;
        battleInfo.missleGunCount = 0;
        battleInfo.fighterCountCur = 0;
        battleInfo.fighterCountMax = 0;

        battleInfo.hpCur = 0;
        battleInfo.hpMax = 0;

        battleInfo.moveSpeed = 0;
        battleInfo.rotateSpeed = 0;
        battleInfo.attackRange = 0;

    }

    public void SetFormationType(FleetFormationType type)
    {
        formationType = type;
    }
    
    public void UpdateBattleInfo()
    {
        SetAbilityZero();
        //ship 정보를 합해 fleet 정보를 만든다.
        for (int i = 0; i < shipInfoList.Count; ++i)
        {
            battleInfo.beamGunCount += shipInfoList[i].battleInfo.beamGunCount;
            battleInfo.missleGunCount += shipInfoList[i].battleInfo.missleGunCount;
            battleInfo.fighterCountCur += shipInfoList[i].battleInfo.fighterCountCur;
            battleInfo.fighterCountMax += shipInfoList[i].battleInfo.fighterCountMax;

            battleInfo.hpCur += shipInfoList[i].battleInfo.hpCur;
            battleInfo.hpMax += shipInfoList[i].battleInfo.hpMax;


            if (battleInfo.moveSpeed == 0 || battleInfo.moveSpeed > shipInfoList[i].battleInfo.moveSpeed)
            {
                battleInfo.moveSpeed = shipInfoList[i].battleInfo.moveSpeed;
            }

            if (battleInfo.rotateSpeed == 0 || battleInfo.rotateSpeed > shipInfoList[i].battleInfo.rotateSpeed)
            {
                battleInfo.rotateSpeed = shipInfoList[i].battleInfo.rotateSpeed;
            }

            if (battleInfo.attackRange == 0 || battleInfo.attackRange > shipInfoList[i].battleInfo.attackRange)
            {
                battleInfo.attackRange = shipInfoList[i].battleInfo.attackRange;
            }

        }

        battleInfo.shieldType = shipInfoList[0].battleInfo.shieldType;//함대는 기함의 실드타입을 씀
        battleInfo.defence = shipInfoList[0].battleInfo.defence;//함대는 기함의 방어 수치 씀




    }


}
