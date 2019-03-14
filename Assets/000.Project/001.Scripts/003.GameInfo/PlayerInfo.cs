using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//서버에서 클라에 주는 정보
public class PlayerInfo
{
    public long usn = 0L;
    public string name = "";
    public CharacterRank characterRank;
    public int exp = 0;
    public int level = 0;

    //자원관련
    public List<AssetInfo> assetInfoList = new List<AssetInfo>();   //자원 정보를 여기에 
    //public int energy;       //자연회복
    //public int gold;         //자연회복
    //public int cube;         //자연회복

    public List<FleetInfo> fleetInfoList = new List<FleetInfo>();


    public void Clear()
    {
        usn = 0L;
        name = "";
        characterRank = CharacterRank.None;
        exp = 0;
        level = 0;

        assetInfoList.Clear();

        fleetInfoList.Clear();

    }

    public void TestInit()
    {
        Clear();

        usn = 5678L;
        name = "";
        characterRank = CharacterRank.None;
        exp = 0;
        level = 0;

        //자원
        AssetInfo aiEnergy = new AssetInfo();
        aiEnergy.type = AssetType.Energy;
        assetInfoList.Add(aiEnergy);

        AssetInfo aiGold = new AssetInfo();
        aiGold.type = AssetType.Gold;
        assetInfoList.Add(aiGold);

        AssetInfo aiCube = new AssetInfo();
        aiCube.type = AssetType.Cube;
        assetInfoList.Add(aiCube);


        //함대
        CreateFleetInfoList();



    }
    private void CreateFleetInfoList()
    {
        fleetInfoList.Clear();

        //1함대
        FleetInfo fInfo = new FleetInfo();
        List<ShipInfo> shipInfoList = new List<ShipInfo>();
        for (int s = 0; s < 5; ++s)//함선 수
        {
            ShipInfo sInfo = new ShipInfo();
            sInfo.DeepCopyInfo(GData.Instance.GetShipInfo("test_d", 1));
            sInfo.linkCur = 3;
            sInfo.linkMax = 5;
            sInfo.belongFleet = 1;
            sInfo.bLockFromDisassemble = false;
            shipInfoList.Add(sInfo);
        }
        fInfo.formationType = FleetFormationType.Wing;
        fInfo.SetShipInfoList(shipInfoList);
        fleetInfoList.Add(fInfo);

        //2함대
        fInfo = new FleetInfo();
        shipInfoList = new List<ShipInfo>();
        for (int s = 0; s < 6; ++s)//함선 수
        {
            ShipInfo sInfo = new ShipInfo();
            sInfo.DeepCopyInfo(GData.Instance.GetShipInfo("test_d", 1));
            sInfo.linkCur = 1;
            sInfo.linkMax = 5;
            sInfo.belongFleet = 2;
            sInfo.bLockFromDisassemble = false;
            shipInfoList.Add(sInfo);
        }
        fInfo.formationType = FleetFormationType.Wing;
        fInfo.SetShipInfoList(shipInfoList);
        fleetInfoList.Add(fInfo);

        //3함대
        fInfo = new FleetInfo();
        shipInfoList = new List<ShipInfo>();
        for (int s = 0; s < 8; ++s)//함선 수
        {
            ShipInfo sInfo = new ShipInfo();
            sInfo.DeepCopyInfo(GData.Instance.GetShipInfo("test_d", 1));
            sInfo.linkCur = 1;
            sInfo.linkMax = 5;
            sInfo.belongFleet = 3;
            sInfo.bLockFromDisassemble = false;
            shipInfoList.Add(sInfo);
        }
        fInfo.formationType = FleetFormationType.Wing;
        fInfo.SetShipInfoList(shipInfoList);
        fleetInfoList.Add(fInfo);

        //4함대
        fInfo = new FleetInfo();
        shipInfoList = new List<ShipInfo>();
        for (int s = 0; s < 3; ++s)//함선 수
        {
            ShipInfo sInfo = new ShipInfo();
            sInfo.DeepCopyInfo(GData.Instance.GetShipInfo("test_d", 1));
            sInfo.linkCur = 1;
            sInfo.linkMax = 5;
            sInfo.belongFleet = 4;
            sInfo.bLockFromDisassemble = false;
            shipInfoList.Add(sInfo);
        }
        fInfo.formationType = FleetFormationType.Wing;
        fInfo.SetShipInfoList(shipInfoList);
        fleetInfoList.Add(fInfo);

        //5함대
        fInfo = new FleetInfo();
        shipInfoList = new List<ShipInfo>();
        for (int s = 0; s < 1; ++s)//함선 수
        {
            ShipInfo sInfo = new ShipInfo();
            sInfo.DeepCopyInfo(GData.Instance.GetShipInfo("test_d", 1));
            sInfo.linkCur = 1;
            sInfo.linkMax = 5;
            sInfo.belongFleet = 5;
            sInfo.bLockFromDisassemble = false;
            shipInfoList.Add(sInfo);
        }
        fInfo.formationType = FleetFormationType.Wing;
        fInfo.SetShipInfoList(shipInfoList);
        fleetInfoList.Add(fInfo);

    }

}