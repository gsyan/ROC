using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampYakSan : GameField
{
    private GameObject _background = null;
    private SequenceProcessor _processor = new SequenceProcessor();



    protected override void OnEnable()
    {
        base.OnEnable();


        SceneLoad.nextScene = "CampYakSan";
    }


    private void Start()
    {
#if UNITY_EDITOR
        GInfo.SetupTest();
        GameTime.Instance.Init();//GameTime의 Instance 초기화를 위해
#endif
        _processor.Add(Test01);
        _processor.Add(Test02);
        _processor.Add(Test03);
        _processor.Add(Test04);
        _processor.Add(Test05);
        StartCampProcessor();


        UISystem.Instance.ShowPanel(GlobalValues.PANEL_ASSET);
        Transform tm = UISystem.Instance.ShowPanel(GlobalValues.PANEL_MAIN);
        if(tm != null)
        {
            UIPanelMain panelMain = tm.GetComponent<UIPanelMain>();
            panelMain.ChangeLayer(SceneLayerType.Camp);
            //panelMain.SetupPlayer(clientPlayer);
            //panelMain.SetupJoystick(clientPlayer);
        }





    }



    private void Test01()
    {
        Debug.Log("Test01");
        _processor.NextProcess();
    }
    private void Test02()
    {
        Debug.Log("Test02");
        _processor.NextProcess();
    }
    private void Test03()
    {
        Debug.Log("Test03");
        _processor.NextProcess();
    }
    private void Test04()
    {
        Debug.Log("Test04");
        _processor.NextProcess();
    }
    private void Test05()
    {
        Debug.Log("Test05");
        _processor.NextProcess();
    }
    


    private void OnTutorialInit(bool isSuccess)
    {
        Debug.Log("OnTutorialInit isSuccess: " + isSuccess);
    }

    private void StartCampProcessor()
    {
        _processor.Start(OnTutorialInit);
    }









    public void ShowFleetManagement()
    {
        Transform tm = UISystem.Instance.ShowPanel(GlobalValues.PANEL_MANAGE_FLEET);
        if (tm != null)
        {
            UIPanelManageFleet panelManageFleet = tm.GetComponent<UIPanelManageFleet>();
            panelManageFleet.SetFleet(GInfo.playerInfo.fleetInfoList);
        }
    }









}
