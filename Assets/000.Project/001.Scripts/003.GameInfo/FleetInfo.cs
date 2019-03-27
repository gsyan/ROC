using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FleetInfo : CharacterInfo
{
    public List<ShipInfo> shipInfoList;

    public FleetFormationType formationType;

    /// <summary>
    /// 초기화 수준의 생성
    /// </summary>
    public override void SetInfo()
    {
        base.SetInfo();

        shipInfoList = new List<ShipInfo>();
        formationType = FleetFormationType.None;
    }


    public override void SetInfo(CharacterInfo ci)
    {
        base.SetInfo(ci);

        FleetInfo fleetInfo = (FleetInfo)ci;
        shipInfoList = fleetInfo.shipInfoList;
        formationType = fleetInfo.formationType;

    }
    //

	
    public void SetShipInfoList(List<ShipInfo> inShipInfoList)
    {
        shipInfoList = inShipInfoList;
        UpdateInfo();
    }
    
    public void SetFormationType(FleetFormationType type)
    {
        formationType = type;
    }
    
    public void UpdateInfo()
    {
        // 정보 초기화
        SetInfo();
        
        //ship 정보를 합해 fleet 정보를 만든다.
        for (int i = 0; i < shipInfoList.Count; ++i)
        {
            attackInfo.beamGunCount += shipInfoList[i].attackInfo.beamGunCount;
            attackInfo.missleGunCount += shipInfoList[i].attackInfo.missleGunCount;
            attackInfo.fighterCountCur += shipInfoList[i].attackInfo.fighterCountCur;
            attackInfo.fighterCountMax += shipInfoList[i].attackInfo.fighterCountMax;
            if (attackInfo.attackRange == 0 || attackInfo.attackRange > shipInfoList[i].attackInfo.attackRange)
            {
                attackInfo.attackRange = shipInfoList[i].attackInfo.attackRange;
            }


            if (moveInfo.moveSpeed == 0 || moveInfo.moveSpeed > shipInfoList[i].moveInfo.moveSpeed)
            {
                moveInfo.moveSpeed = shipInfoList[i].moveInfo.moveSpeed;
            }
            if (moveInfo.rotateSpeed == 0 || moveInfo.rotateSpeed > shipInfoList[i].moveInfo.rotateSpeed)
            {
                moveInfo.rotateSpeed = shipInfoList[i].moveInfo.rotateSpeed;
            }

            hpCur += shipInfoList[i].hpCur;
            hpMax += shipInfoList[i].hpMax;
        }

        defenceInfo.shieldType = shipInfoList[0].defenceInfo.shieldType;//함대는 기함의 실드타입을 씀
        defenceInfo.defence = shipInfoList[0].defenceInfo.defence;//함대는 기함의 방어 수치 씀




    }


}
