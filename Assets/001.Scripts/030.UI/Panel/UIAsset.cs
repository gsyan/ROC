using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIAsset : MonoBehaviour
{
    [SerializeField]
    private Text _textEnergy = null;
    [SerializeField]
    private Text _textEnergyState = null;
    [SerializeField]
    private Text _textGold = null;
    [SerializeField]
    private Text _textCube = null;

    private AssetInfo _energyInfo;
    private AssetInfo _goldInfo;
    private AssetInfo _cubeInfo;
    private readonly float _updateInterval = 3.0f;
    private float _elipsedTime = 0.0f;
    private bool _bEnergyMax = false;
    private bool _updateData = false;


    private void OnEnable()
    {
        UpdateData();
        Messenger.AddListener(EventKey.UpdateAssetInfo, UpdateData);
    }
    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.UpdateAssetInfo, UpdateData);
    }
    private void UpdateData()
    {
        _elipsedTime = 0.0f;
        //자원 표시량 업데이트
        EnergyUpdateData();
        GoldUpdateData();
        CubeUpdateData();
    }
    private void EnergyUpdateData()
    {
        _energyInfo = GInfo.GetAssetInfo(AssetType.Energy);
        if (_energyInfo != null)
        {
            if (_energyInfo.curCount >= GlobalValues.energyCountMax)
            {
                if (_textEnergyState != null)
                {
                    _textEnergyState.text = Localization.Get("energy_max");
                }
            }
        }
        else//에너지 정보 없으면 리턴
        {
            return;
        }


        if (_textEnergy != null)
        {
            int energyCount = _energyInfo.curCount;
            int extraEnergyCount = _energyInfo.extraCount;

            string s0 = Localization.Format("gagebar", energyCount, GlobalValues.energyCountMax);
            string s1 = extraEnergyCount > 0 ? string.Format("+{0}", extraEnergyCount) : "";

            _textEnergy.text = string.Format("{0} {1}", s0, s1);
        }
    }
    private void GoldUpdateData()
    {
        _goldInfo = GInfo.GetAssetInfo(AssetType.Gold);
        if (_goldInfo != null)
        { }
        else//정보 없으면 리턴
        {
            return;
        }

        if (_textGold != null)
        {
            _textGold.text = _goldInfo.curCount.ToString();
        }
    }
    private void CubeUpdateData()
    {
        _cubeInfo = GInfo.GetAssetInfo(AssetType.Cube);
        if (_cubeInfo != null)
        { }
        else//정보 없으면 리턴
        {
            return;
        }

        if (_textCube != null)
        {
            _textCube.text = _cubeInfo.curCount.ToString();
        }

    }

    private void Update()
    {
        if (_energyInfo.curCount < GlobalValues.energyCountMax ||
             _goldInfo.curCount < GlobalValues.goldRechargeMax)
        {
            _elipsedTime += GameTime.deltaTime;
            if (_elipsedTime < _updateInterval) { return; }
            _elipsedTime = 0.0f;
            //Debug.Log("_elipsedTime: 0");
            EnergyUpdate();
            GoldUpdate();
            CubeUpdate();
        }
        else
        {

        }
    }
    private void EnergyUpdate()
    {//자원 회복 카운트 & req
        if (_energyInfo != null)
        {
            if (_energyInfo.curCount >= GlobalValues.energyCountMax)
            {
                _textEnergyState.text = Localization.Get("max");
                return;
            }
            else
            {
                if (GInfo.serverTime <= _energyInfo.nextChageTime)
                {
                    TimeSpan remainTime = _energyInfo.nextChageTime - GInfo.serverTime;
                    if (_textEnergyState != null)
                    {
                        _textEnergyState.text = string.Format("{0}:{1}", remainTime.Minutes, remainTime.Seconds);
                    }
                }
                else
                {
                    LobbyManager.Instance.RefreshEnergyReq();
                }

            }
        }

    }
    private void GoldUpdate()
    {

    }
    private void CubeUpdate()
    {

    }

    public void OpenStore(string storeName)
    {
        //Transform tm = UISystem.Instance.ShowPanel(GlobalValues.PANEL_CASHSTORE);
        //UIPanelCashStore store = tm.GetComponent<UIPanelCashStore>();
        //store.OnSelectTab(ProductType.Cube);

        Debug.Log("storeName: " + storeName);
    }



}
