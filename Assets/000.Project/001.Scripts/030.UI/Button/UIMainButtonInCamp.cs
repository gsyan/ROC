using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainButtonInCamp : MonoBehaviour
{


    public void OnButtonBattle()
    {
        Transform tm = BKST.UISystemBK.Instance.ShowPanel(GlobalValues.PANEL_BATTLE_MENU);
        if (tm != null)
        {
            //UIPanelManageFleet panelManageFleet = tm.GetComponent<UIPanelManageFleet>();
            //panelManageFleet.SetFleet(GInfo.playerInfo.fleetInfoList);
        }
    }

    public void OnButtonFleetManage()
    {
        Transform tm = BKST.UISystemBK.Instance.ShowPanel(GlobalValues.PANEL_MANAGE_FLEET);
        if (tm != null)
        {
            UIPanelManageFleet panelManageFleet = tm.GetComponent<UIPanelManageFleet>();
            panelManageFleet.SetFleet(GInfo.playerInfo.fleetInfoList);
        }
    }
	


}
