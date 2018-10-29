using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// canvas scaler 의 UI scale mode (scale with screen size 이용: 해상도에 따라 UI 상대적 크기, 배치 형태 유지
/// </summary>
public enum UITypeBK
{
    None = -1,
    Overlay,
    Camera,
    Max
}
namespace BKST
{
    public class UISystem : MonoBehaviour
    {
        private static UISystem _instance;
        public static UISystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UISystem>();
                    if(_instance == null)
                    {
                        GameObject obj = Utility.Instantiate("UI/UISystem") as GameObject;
                        _instance = obj.GetComponent<UISystem>();
                        DontDestroyOnLoad(obj);
                        obj.name = "UISystem";

                        if (GameObject.Find("EventSystem") == null)
                        {
                            obj = new GameObject("EventSystem");
                            DontDestroyOnLoad(obj);
                            obj.transform.parent = null;
                            obj.transform.position = Vector3.zero;
                            obj.AddComponent<EventSystem>();
                            obj.AddComponent<StandaloneInputModule>();
                        }
                    }
                }
                return _instance;
            }
        }

        private CanvasBK[] canvas;

        private List<Transform> _createdPanelList = new List<Transform>();//만들어진 패널 리스트

        private void Awake()
        {
            canvas = new CanvasBK[(int)UITypeBK.Max];
            canvas[0] = transform.Find("CanvasOverlay").GetComponent<CanvasBK>();
            canvas[1] = transform.Find("CanvasCamera").GetComponent<CanvasBK>();

            Localization.LoadCSV(ResourceSystem.Load("Localization/localization", true) as TextAsset, true);
            Localization.language = PlayerPrefs.GetString("language", "korean");
            LanguageCheck();
        }
        private void LanguageCheck()
        {
            SystemLanguage sl = Application.systemLanguage;
            RuntimePlatform rp = Application.platform;
            if (sl == SystemLanguage.Korean)
            {
                Localization.language = "Korean";
            }
            else if (sl == SystemLanguage.Japanese)
            {
                Localization.language = "Japanese";
            }
            else
            {
                Localization.language = "English";
            }
        }
        private void OnDestroy()
        {
            _instance = null;
        }
        private void OnLevelWasLoaded(int level)
        {
            DLog.LogMSG("UISystem / OnLevelWasLoaded()-> level: " + level + " frameCount: " + Time.frameCount);

            int fCount = _createdPanelList.Count;
            for (int i = fCount - 1; i >= 0; --i)
            {
                UIPanelBase pb = _createdPanelList[i].GetComponent<UIPanelBase>();
                if (pb != null)
                {
                    switch (pb.manageType)
                    {
                        case PanelManageType.None:
                            _createdPanelList.RemoveAt(i);
                            break;
                    }
                }
                else
                {
                    _createdPanelList.RemoveAt(i);
                }
            }
        }
        public void RemoveAll()
        {
            for(int i = 0; i< (int)UITypeBK.Max; ++i)
            {
                canvas[i].RemoveAll();
            }
            _createdPanelList.Clear();//각 캔바스가 가진 목록을 총괄 개념으로 중복으로 가지고 있던 것이기 때문에 그냥 클리어

            //uisystem 오브젝트가 에셋번들 자원을 참조하기 때문
            //에셋번들 release 하는 때에 그 참조된 값들이 null 화 된다.
            //그때에 uisystem도 removeAll 하니, 아예 오브젝트 까지 없애고 새로 instance 시키는 방법을 취한다.
            DestroyImmediate(_instance.gameObject);
            _instance = null;
        }

        public void SetInputState(bool b)
        {
            for (int i = 0; i < (int)UITypeBK.Max; ++i)
            {
                canvas[i].GetComponent<CanvasGroup>().interactable = b;
            }
        }

        public Transform ShowMessageBox()
        {
            string panelName = "Panel MessageBox";
            Transform tm = FindPanel(panelName);
            if (tm != null && tm.gameObject.activeSelf)
            {
                canvas[0].RestoreBlockScreen(tm.GetComponent<UIPanelBase>());
                tm.gameObject.SetActive(false);
            }
            return ShowPanel(panelName);
        }

        public Transform ShowPanel(string panelName)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogWarning("UISystem.ShowPanel() panelName is null or empty."); return null;
            }

            bool created = false;

            Transform tm = FindPanel(panelName);//생성된 것들 중 찾아보고
            if (tm == null)//없으면 
            {
                tm = CreatePanel(panelName);//만든다.
                if (tm == null)
                {
                    Debug.LogWarning("UISystem.ShowPanel() create panel fail. panelName:" + panelName);
                    return null;
                }

                created = true;
            }

            //새로 만들었거나 | 액티브 상태가 아니거나
            if (created || !tm.gameObject.activeSelf)
            {
                UIPanelBase pb = tm.GetComponent<UIPanelBase>();
                switch (pb.uiType)
                {
                    case UITypeBK.Overlay:
                        canvas[0].ShowPanel(pb);
                        break;
                    case UITypeBK.Camera:
                        canvas[0].GetComponent<Canvas>().enabled = false;
                        canvas[1].ShowPanel(pb);
                        break;
                }
            }

            return tm;
        }
        public Transform FindPanel(string panelName)
        {
            return _createdPanelList.Find(delegate (Transform tm) {
                return tm.name == panelName;
            });
        }
        private Transform CreatePanel(string panelName)
        {
            GameObject go = Utility.Instantiate("UI/" + panelName) as GameObject;
            if (go != null)
            {
                Transform tm = go.transform;
                tm.name = panelName;

                UIPanelBase pb = tm.GetComponent<UIPanelBase>();
                switch (pb.uiType)
                {
                    case UITypeBK.Overlay:
                        canvas[(int)UITypeBK.Overlay].AddPanel(tm);
                        break;
                    case UITypeBK.Camera:
                        canvas[(int)UITypeBK.Camera].AddPanel(tm);
                        break;
                }
                _createdPanelList.Add(tm);

                return tm;
            }
            return null;
        }

        public void HidePanel(string panelName, bool immediate)
        {
            HidePanel(FindPanel(panelName), immediate);
        }
        public void HidePanel(Transform tm, bool immediate)
        {
            if (tm == null) return;
            UIPanelBase pb = tm.GetComponent<UIPanelBase>();
            switch (pb.uiType)
            {
                case UITypeBK.Overlay:
                    canvas[0].HidePanel(pb, immediate);
                    break;
                case UITypeBK.Camera:
                    canvas[1].HidePanel(pb, immediate);
                    if(!canvas[1].IsExistToHide())
                    {
                        canvas[0].GetComponent<Canvas>().enabled = true;
                        canvas[1].GetComponent<Canvas>().enabled = false;
                    }
                    break;
            }
        }


        public bool IsExistToHide()//Escape
        {
            int activeCanvas = 0;
            for( int i = 0; i < (int)UITypeBK.Max; ++i)
            {
                activeCanvas = i;
                if (canvas[i].GetComponent<Canvas>().enabled == true )
                {
                    break;
                }
            }
            return canvas[activeCanvas].IsExistToHide();
        }


    }
}


