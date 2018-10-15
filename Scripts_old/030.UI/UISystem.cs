using UnityEngine;
using System.Collections.Generic;

public class UISystem : MonoBehaviour
{
    public static UISystem _instance;
    public static UISystem Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UISystem>();
                if( _instance == null )
                {
                    //게임 헬퍼, utility, gameinfo 를 끝내고 이부분 시작
                    GameObject go = Utility.Instantiate("UI/UI System") as GameObject;
                    _instance = go.GetComponent<UISystem>();
                    DontDestroyOnLoad(go);
                    go.name = "UISystem";
                }
            }
            return _instance;
        }
    }

    [HideInInspector] public Camera mainCamera;
    private UICamera[] _uiCamera;

    
    private List<Transform> _createdPanelList = new List<Transform>();//만들어진 패널 리스트
    private Stack<UIPanelBase> _panelStack = new Stack<UIPanelBase>();//기존에 열려있던것들 안보이게 하고 스택에 넣는 용
    private List<UIPanelBase> _blockPanelList = new List<UIPanelBase>();


    //종류별 현재 패널
    private UIPanelBase _currentMain = null;
    private UIPanelBase _currentSub = null;
    private UIPanelBase _currentPopup = null;
    private UIPanelBase _currentTooltip = null;

    private UIBlackImageController _blockScreen = null;
    private UIColorSetting _colorSetting = null;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
        _uiCamera = GetComponentsInChildren<UICamera>();
        _blockScreen = GetComponentInChildren<UIBlackImageController>();

        AdjustResolution();//기기별 해상도 맞추기
        
        Localization.LoadCSV(ResourceSystem.Load("Localization/localization", true)as TextAsset, true);
        Localization.language = PlayerPrefs.GetString("language", "korean");
        //bk: 디바이스 상황에 따라 언어를 선택하려면 주석처리 하고, 아래 내용으로 대체 해야 함
        //SystemLanguage sl = Application.systemLanguage;
        //RuntimePlatform rp = Application.platform;
        //BKUtil.Instance.Log("SystemLanguage: " + sl.ToString());
        //if (sl == SystemLanguage.Korean)
        //{
        //    Localization.language = "Korean";//English, Korean, Japanese
        //}
        //else if (sl == SystemLanguage.Japanese)
        //{
        //    Localization.language = "Japanese";
        //}
        //else
        //{
        //    Localization.language = "English";
        //}
    }
    private void AdjustResolution()
    {
        UIRoot uiRoot = transform.GetComponent<UIRoot>();
        if (uiRoot != null)
        {
            uiRoot.manualWidth = Screen.width;
            uiRoot.manualHeight = Screen.height;

            //필요에 따라서 fit 이용
            //uiRoot.fitHeight = true;
            //uiRoot.fitWidth = true;
        }
    }


    private void OnDestroy()
    {
        _instance = null;
    }

    public UIColorSetting GetColorSetting()
    {
        if(_colorSetting == null)
        {
            _colorSetting = GetComponent<UIColorSetting>();
        }
        return _colorSetting;
    }

    private void OnLevelWasLoaded(int level)
    {
        DLog.LogMSG("UISystem / OnLevelWasLoaded()-> level: " + level + " frameCount: " + Time.frameCount);

        _blockPanelList.Clear();
        RestoreBlockScreen();

        int fCount = _createdPanelList.Count;
        for (int i = fCount - 1; i >= 0; --i)
        {
            UIPanelBase pb = _createdPanelList[i].GetComponent<UIPanelBase>();
            if(pb != null)
            {
                if(!pb.isDeactiveCall)
                {
                    pb.OnDeactive();
                }

                switch(pb.manageType)
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
        if(tm != null)
        {
            tm.GetComponent<UIPanelMain>().RemoveOnSceneLoad();//UIPanelMain 와 LayeredPrefab 을 완성해야 한다.
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
        for (int i=0; i< fCount; ++i)
        {
            if(_createdPanelList[i] != null)
            {
                DestroyImmediate(_createdPanelList[i].gameObject);
            }
        }
        _createdPanelList.Clear();
        
        //bk: uisystem 오브젝트가 에셋번들 자원을 참조하기 때문
        //에셋번들 release 하는 때에 그 참조된 값들이 null 화 된다.
        //그때에 uisystem도 removeAll 하니, 아예 오브젝트 까지 없애고 새로 instance 시키는 방법을 취한다.
        DestroyImmediate(_instance.gameObject);
        _instance = null;
        //bk end;
    }


    public Transform ShowMessageBox()
    {
        string panelName = "Panel MessageBox";
        Transform tm = FindPanel(panelName);

        if(tm != null && tm.gameObject.activeSelf)
        {
            RestoreBlockScreen(tm.GetComponent<UIPanelBase>());
            tm.gameObject.SetActive(false);
        }

        return ShowPanel(panelName);
    }



    
    //인풋을 막거나 / 허용
    public void SetInputState(bool b)
    {
        if(_uiCamera != null)
        {
            for(int i =0; i< _uiCamera.Length; ++i)
            {
                _uiCamera[i].useKeyboard = b;
                _uiCamera[i].useMouse = b;
                _uiCamera[i].useTouch = b;
            }
        }
    }

    public bool IsExistToHide()//Escape
    {
        if(_currentTooltip != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentTooltip = " + _currentTooltip.name);
            HidePanel(_currentTooltip.cachedTransform);
            return true;
        }

        if (_currentPopup != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentPopup = " + _currentPopup.name);
            HidePanel(_currentPopup.cachedTransform);
            return true;
        }
        
        if (_currentSub != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentSub = " + _currentSub.name);
            HidePanel(_currentSub.cachedTransform);
            return true;
        }
        
        if (_currentMain != null)
        {
            DLog.LogMSG("UISystem.Escape() _currentMain = " + _currentMain.name);
            HidePanel(_currentMain.cachedTransform);
            return true;
        }
        
        return false;
    }




    private bool _releaseJoypad = false;
    public Transform ShowPanel(string panelName, bool refresh = false)
    {
        _releaseJoypad = false;
        bool created = false;

        if(string.IsNullOrEmpty(panelName))
        {
            Debug.LogWarning("UISystem.ShowPanel() panelName is null or empty.");
            return null;
        }

        Transform tm = FindPanel(panelName);//생성된 것들 중 찾아보고
        if(tm == null)//없으면 
        {
            tm = CreatePanel(panelName);//만든다.
            if(tm == null)
            {
                Debug.LogWarning("UISystem.ShowPanel() create panel fail. panelName:" + panelName);
            }

            created = true;

            // 패널 스케일 조정 필요하다면 사용
            //tm.localScale = new Vector3(2.9f, 2.9f, 2.9f);
        }
        
        //새로 만들었거나 | 액티브 상태가 아니거나
        if(created || !tm.gameObject.activeSelf)
        {
            UIPanelBase pb = tm.GetComponent<UIPanelBase>();
            if(pb != null)//UIPanelBase 컴포넌트가 있다면
            {
                switch(pb.type)
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

                if(pb.blockScreen)
                {
                    _blockScreen.Setup(pb);
                    _blockPanelList.Add(pb);
                }
            }
            else//UIPanelBase 컴포넌트가 없다면
            {
                if(refresh)
                {
                    tm.gameObject.SetActive(false);
                }
                tm.gameObject.SetActive(true);
            }

            if (_releaseJoypad && UIVirtualPad.current != null)
            {
                UIVirtualPad.current.Release();
            }
        }

        return tm;
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
            if(_currentSub != null)
            {
                _panelStack.Push(_currentSub);//uisystem 의 패널스택에 기존 서브 패널 stack
                _currentSub.gameObject.SetActive(false);//stacked 서브 패널 비활성화
            }
        }

        _currentMain = pb;
        _currentSub = null;
        _releaseJoypad = true;
    }
    private void ShowSubPanel(UIPanelBase pb)
    {
        if(_currentSub != null)//기존 서브 패널 있다면
        {
            if(_currentMain != null)//현재 메인 패널 있다면
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
        if(_currentPopup != null)
        {
            _currentPopup.gameObject.SetActive(false);//기존 _currentPopup 을 비활성화
            if (_currentPopup.blockScreen)
            {
                RestoreBlockScreen(_currentPopup);//기존 _currentPopup 을 _blockPanelList 에서 제거
            }

            _currentPopup = pb;
            _releaseJoypad = true;
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
        _releaseJoypad = true;
    }
    private void RestoreBlockScreen(UIPanelBase current = null)
    {
        if(_blockScreen != null)
        {
            if(current != null)
            {
                _blockPanelList.Remove(current);
                current = null;

                if(_blockPanelList.Count > 0)
                {
                    current = _blockPanelList[_blockPanelList.Count - 1];
                }
            }

            _blockScreen.Setup(current);
        }
    }
    
    public void HidePanel(string panelName, bool immediate = false)
    {
        HidePanel(FindPanel(panelName), immediate);
    }
    public void HidePanel(Transform tm, bool immediate = false)
    {
        if (tm == null) return;

        UIPanelBase pb = tm.GetComponent<UIPanelBase>();
        if(pb != null)
        {
            Callback onRestoreBlockScreen = () =>
            {
                if (pb.blockScreen)
                {
                    RestoreBlockScreen(pb);
                }
            };

            switch(pb.type)
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
        else
        {
            tm.gameObject.SetActive(false);
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

        if(_currentMain != null)
        {
            _currentSub = _currentMain.PopSubPanel();
            if(_currentSub != null)
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
        switch(pb.type)
        {
            case PanelType.Main:
                _currentMain = pb;
                _currentSub = pb.PopSubPanel();
                if(_currentSub != null)
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



    public Transform FindPanel(string panelName)
    {
        return _createdPanelList.Find(delegate (Transform tm) {
            return tm.name == panelName;
        });
    }
    private Transform CreatePanel(string panelName)
    {
        GameObject go = Utility.Instantiate("UI/" + panelName)as GameObject;
        if(go != null)
        {
            Transform tm = go.transform;
            tm.name = panelName;
            Utility.SetParent(transform, tm, false);
            _createdPanelList.Add(tm);
            return tm;
        }
        return null;
    }











}
