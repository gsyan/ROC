using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBK : MonoBehaviour
{
    public UITypeBK uiType = UITypeBK.Overlay;

    private UIDepthBK[] _depthArray;

    private Transform _blackImage;
    private Transform _blackImageParent;

    private List<Transform> _createdPanelList = new List<Transform>();//만들어진 패널 리스트
    private Stack<UIPanelBase> _panelStack = new Stack<UIPanelBase>();//기존에 열려 있던 것들 안보이게 하고 스택에 넣는 용
    private List<UIPanelBase> _blockPanelList = new List<UIPanelBase>();//기존에 검은 반투명 이미지에 가려졌던 것, 최신의 것이 있어 다음 차례로 예약된 것들

    private UIPanelBase _currentMain = null;
    private UIPanelBase _currentSub = null;
    private UIPanelBase _currentPopup = null;
    private UIPanelBase _currentTooltip = null;

    private UIBlackImageController _blackImageController = null;
    private UIColorSetting _colorSetting = null;

    private void Awake()
    {
        _depthArray = GetComponentsInChildren<UIDepthBK>();
        _blackImageController = GetComponentInChildren<UIBlackImageController>();
    }
    private void OnLevelWasLoaded(int level)
    {
        DLog.LogMSG("CanvasBK / OnLevelWasLoaded()-> level: " + level + " frameCount: " + Time.frameCount);

        _blockPanelList.Clear();
        RestoreBlockScreen(null);

        int fCount = _createdPanelList.Count;
        for (int i = fCount - 1; i >= 0; --i)
        {
            UIPanelBase pb = _createdPanelList[i].GetComponent<UIPanelBase>();
            if (pb != null)
            {
                if (!pb.isDeactiveCall)
                {
                    pb.OnDeactive();
                }

                switch (pb.manageType)
                {
                    case PanelManageType.None:
                        DestroyImmediate(pb.gameObject);
                        _createdPanelList.RemoveAt(i);
                        break;
                    case PanelManageType.DontDestory:
                        _createdPanelList[i].gameObject.SetActive(false);
                        break;
                }
            }
            else
            {
                DestroyImmediate(_createdPanelList[i].gameObject);
                _createdPanelList.RemoveAt(i);
            }
        }

        Transform tm = FindPanel(GlobalValues.PANEL_MAIN);
        if (tm != null)
        {
            tm.GetComponent<UIPanelMain>().RemoveOnSceneLoad();
        }

        _panelStack.Clear();

        _currentMain = null;
        _currentSub = null;
        _currentPopup = null;
        _currentTooltip = null;
    }
    public void RemoveAll()
    {
        int fCount = _createdPanelList.Count;
        for (int i = 0; i < fCount; ++i)
        {
            if (_createdPanelList[i] != null)
            {
                DestroyImmediate(_createdPanelList[i].gameObject);
            }
        }
        _createdPanelList.Clear();
    }

    
    
   
    /// <summary>
    /// current 가 null 이면 아무것도 안가려진 기본 상태가 되는것
    /// </summary>
    /// <param name="current"></param>
    public void RestoreBlockScreen(UIPanelBase current)
    {
        if (_blackImageController != null)
        {
            if (current != null)
            {
                _blockPanelList.Remove(current);
                current = null;

                if (_blockPanelList.Count > 0)
                {
                    current = _blockPanelList[_blockPanelList.Count - 1];
                }
            }

            _blackImageController.Setup(current);
        }
    }

    public void AddPanel(Transform tm)
    {
        UIPanelBase pb = tm.GetComponent<UIPanelBase>();
        Utility.SetParentUI(_depthArray[pb.depth].transform, tm, false);
        _createdPanelList.Add(tm);
    }
    public void ShowPanel(UIPanelBase pb)
    {
        transform.GetComponent<Canvas>().enabled = true;

        switch (pb.type)
        {
            case PanelType.Main:
                ShowMainPanel(pb);
                break;
            case PanelType.Sub:
                ShowSubPanel(pb);
                break;
            case PanelType.Popup:
                ShowPopupPanel(pb);
                break;
            case PanelType.ToolTip:
                ShowTooltipPanel(pb);
                break;
        }

        pb.Show();

        if (pb.blockScreen)
        {
            _blackImageController.Setup(pb);
            _blockPanelList.Add(pb);
        }
    }
    private void ShowMainPanel(UIPanelBase pb)
    {
        if (_currentMain != null)//기존 메인 패널 있음
        {
            if (_currentSub != null)// 기존 서브 패널도 있음
            {
                _currentMain.PushSubPanel(_currentSub);//기존 메인 패널에 서브 패널 stack
                _currentSub.gameObject.SetActive(false);//stacked 서브 패널 비활성화
            }

            _panelStack.Push(_currentMain);//uisystem 의 패널스택에 기존 메인 패널 stack
            _currentMain.gameObject.SetActive(false);//stacked 기존 메인 패널 비활성화
        }
        else//기존 메인 패널이 없다면
        {
            if (_currentSub != null)
            {
                _panelStack.Push(_currentSub);//uisystem 의 패널스택에 기존 서브 패널 stack
                _currentSub.gameObject.SetActive(false);//stacked 서브 패널 비활성화
            }
        }

        _currentMain = pb;
        _currentSub = null;
    }
    private void ShowSubPanel(UIPanelBase pb)
    {
        if (_currentSub != null)//기존 서브 패널 있다면
        {
            if (_currentMain != null)//현재 메인 패널 있다면
            {
                _currentMain.PushSubPanel(_currentSub);//현재 메인 패널에 stack
            }
            else//현재 메인 패널이 없다면
            {
                _panelStack.Push(_currentSub);//uisystem의 패널스택에 stack
            }

            _currentSub.gameObject.SetActive(false);
        }

        _currentSub = pb;
    }
    private void ShowPopupPanel(UIPanelBase pb)
    {
        if (_currentPopup != null)
        {
            _currentPopup.gameObject.SetActive(false);//기존 _currentPopup 을 비활성화
            if (_currentPopup.blockScreen)
            {
                RestoreBlockScreen(_currentPopup);//기존 _currentPopup 을 _blockPanelList 에서 제거
            }

            _currentPopup = pb;
        }
    }
    private void ShowTooltipPanel(UIPanelBase pb)
    {
        if (_currentTooltip != null)
        {
            _currentTooltip.gameObject.SetActive(false);
            if (_currentTooltip.blockScreen)
            {
                RestoreBlockScreen(_currentTooltip);//기존 _currentTooltip 을 _blockPanelList 에서 제거
            }
        }

        _currentTooltip = pb;
    }
    

    public void HidePanel(UIPanelBase pb, bool immediate)
    {
        Callback onRestoreBlockScreen = () =>
        {
            if (pb.blockScreen)
            {
                RestoreBlockScreen(pb);
            }
        };

        switch (pb.type)
        {
            case PanelType.None:
                pb.Hide(onRestoreBlockScreen, immediate);
                break;

            case PanelType.Main:
                pb.Hide(HideMainPanel + onRestoreBlockScreen, immediate);
                break;

            case PanelType.Sub:
                pb.Hide(HideSubPanel + onRestoreBlockScreen, immediate);
                break;

            case PanelType.Popup:
                _currentPopup = null;
                pb.Hide(onRestoreBlockScreen, immediate);
                break;

            case PanelType.ToolTip:
                _currentTooltip = null;
                pb.Hide(onRestoreBlockScreen, immediate);
                break;
        }
    }
    private void HideMainPanel()
    {
        _currentMain = null;
        _currentSub = null;
        PopNextPanel();
    }
    private void HideSubPanel()
    {
        _currentSub = null;

        if (_currentMain != null)
        {
            _currentSub = _currentMain.PopSubPanel();
            if (_currentSub != null)
            {
                _currentSub.gameObject.SetActive(true);
            }
        }
        else
        {
            PopNextPanel();
        }
    }
    private void PopNextPanel()
    {
        if (_panelStack.Count < 1) return;
        
        UIPanelBase pb = _panelStack.Pop();
        switch (pb.type)
        {
            case PanelType.Main:
                _currentMain = pb;
                _currentSub = pb.PopSubPanel();
                if (_currentSub != null)
                {
                    _currentSub.gameObject.SetActive(true);
                }
                break;

            case PanelType.Sub:
                _currentSub = pb;
                break;
        }

        pb.gameObject.SetActive(true);
    }

    private Transform FindPanel(string panelName)
    {
        return _createdPanelList.Find(delegate (Transform tm) {
            return tm.name == panelName;
        });
    }


    public UIColorSetting GetColorSetting()
    {
        if (_colorSetting == null)
        {
            _colorSetting = GetComponent<UIColorSetting>();
        }
        return _colorSetting;
    }


    public bool IsExistToHide()//Escape
    {
        if (_currentTooltip != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentTooltip = " + _currentTooltip.name);
            HidePanel(_currentTooltip, false);
            return true;
        }

        if (_currentPopup != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentPopup = " + _currentPopup.name);
            HidePanel(_currentPopup, false);
            return true;
        }

        if (_currentSub != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentSub = " + _currentSub.name);
            HidePanel(_currentSub, false);
            return true;
        }

        if (_currentMain != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentMain = " + _currentMain.name);
            HidePanel(_currentMain, false);
            return true;
        }

        return false;
    }


}
