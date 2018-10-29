using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManageFleet : UIPanelBase
{
    private List<FleetInfo> _fleetInfoList;
    private List<ShipInfo> _shipInfoList;



    private int _tapIndex = 0;                  //현재 선택된 탭
    private List<UIPanelItemManageFleetShipInfo> _uiShipInfoList = new List<UIPanelItemManageFleetShipInfo>();

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        _uiShipInfoList.Clear();
    }

    public void SetFleet(List<FleetInfo> inFleetinfoList)
    {
        if (_uiShipInfoList.Count < 1)//함선 UI 미리 마련
        {
            UIPanelItemManageFleetShipInfo shipInfoUI;
            for (int i = 0; i < 8; ++i)
            {
                shipInfoUI = CreateItem().GetComponent<UIPanelItemManageFleetShipInfo>();
                //Utility.SetParent(shipInfoGrid, shipInfoUI.transform, true);
                _uiShipInfoList.Add(shipInfoUI);
            }
        }


        _fleetInfoList = inFleetinfoList;

        if (_fleetInfoList.Count > 0)
        {
            UpdateUI();
        }







    }
    private GameObject CreateItem()
    {
        return Utility.Instantiate("UI/PanelItem ManageFleet ShipInfo") as GameObject;
    }
    private void DisableAllShipInfoUI()
    {
        int count = _uiShipInfoList.Count;
        if( count > 0)
        {
            for( int i=0; i<count; ++i)
            {
                _uiShipInfoList[i].VisibleUI(false);
            }
        }
    }





    private void Start()
    {
        
    }

    private void UpdateUI()
    {
        DisableAllShipInfoUI();//ui 락상태

        UIPanelItemManageFleetShipInfo shipInfoUI;

        for (int i = 0; i < _fleetInfoList[_tapIndex].shipInfoList.Count; ++i)
        {
            shipInfoUI = _uiShipInfoList[i];
            shipInfoUI.VisibleUI(true);//ui 릴리즈 상태
            shipInfoUI.SetInfo(_fleetInfoList[_tapIndex].shipInfoList[i]);
        }

        //scrollViewShip.ResetPosition();//스크롤뷰 위치 리셋
    }



    public void OnSelectFleetButton(int index)
    {
        Debug.Log("OnSelectFleetButton index: " + index);
        _tapIndex = index;

        UpdateUI();

    }


    public void OnCloseButton()
    {
        BKST.UISystem.Instance.HidePanel(this.transform, false);
    }






	
}
