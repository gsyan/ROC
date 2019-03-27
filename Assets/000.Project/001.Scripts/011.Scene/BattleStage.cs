using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.sbp.ai;

public class BattleStage : GameField
{
    private GameObject _background = null;
    private SequenceProcessor _processor = new SequenceProcessor();

    private List<Fleet> _fleetList = new List<Fleet>();

    private Transform[] spawnPositionBlue;
    private Transform[] spawnPositionRed;
    private Transform[] spawnPositionGreen;




    protected override void OnEnable()
    {
        base.OnEnable();


    }
    private void Start()
    {
#if UNITY_EDITOR
        GInfo.SetupTest();
#endif
        _processor.Add(CreateFleetObject);

        StartCampProcessor();

        //UISystem.Instance.ShowPanel(GlobalValues.PANEL_ASSET);

        Transform tm = UISystem.Instance.ShowPanel(GlobalValues.PANEL_MAIN);
        if (tm != null)
        {
            UIPanelMain panelMain = tm.GetComponent<UIPanelMain>();
            panelMain.ChangeLayer(SceneLayerType.BattleStage);
            //panelMain.SetupPlayer(clientPlayer);
            //panelMain.SetupJoystick(clientPlayer);
        }
    }
    private void CreateFleetObject()
    {
        BattleStageSpawnPosition bssp = (Utility.Instantiate("Character/BattleStage_SpwanPositoin") as GameObject).GetComponent<BattleStageSpawnPosition>();

        Fleet fleet01 = (Utility.Instantiate("Character/Fleet") as GameObject).GetComponent<Fleet>();
        fleet01.SetInfo(GInfo.playerInfo.fleetInfoList[0]);
        //fleet01.SetTeamType(TeamType.Blue);
        //fleet01.SetSpawnPosition(bssp.GetSpawnPosition(TeamType.Blue)[0]);
        //fleet01.SetFleetList(ref _fleetList);
        //_fleetList.Add(fleet01);


        //Fleet_Back fleet04 = (Utility.Instantiate("ObjectPrefab/Fleet") as GameObject).GetComponent<Fleet_Back>();
        //fleet04.SetFleetInfo(GInfo.playerInfo.fleetInfoList[1]);
        //fleet04.SetTeamType(TeamType.Red);
        //fleet04.SetSpawnPosition(bssp.GetSpawnPosition(TeamType.Red)[0]);
        //fleet04.SetFleetList(ref _fleetList);
        //_fleetList.Add(fleet04);


        

        PoolSystem.LoadParticle("spark_1");
        


        _processor.NextProcess();
    }


    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.A) )
        {
            PoolSystem.SpawnParticle("spark_1");
        }

    }



    private void OnCompletePreProcess(bool isSuccess)
    {

    }

    private void StartCampProcessor()
    {
        _processor.Start(OnCompletePreProcess);
    }











}
