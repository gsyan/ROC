using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager _instance;
    public static LobbyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = GameObject.FindObjectOfType(typeof(LobbyManager)) as LobbyManager;
                _instance = FindObjectOfType<LobbyManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("_LobbyManager");
                    _instance = go.AddComponent<LobbyManager>();

                    DontDestroyOnLoad(go);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {

    }

    private void Start()
    {
        SetupLoginProcess();
        
    }


    //  Login Part  ///////////////////////////////////////
    private SequenceProcessor _loginProcessor = new SequenceProcessor();

    /// <summary>
    /// _loginProcessor 에 LobbyManager 차원의 미리 해둬야 할 작업들을 등록해 두기
    /// </summary>
    private void SetupLoginProcess()
    {
        _loginProcessor.Clear();
        _loginProcessor.Add(ProcessGlobalValues);
        _loginProcessor.Add(ProcessNoticeBannerList);
        

    }
    private void ProcessGlobalValues()
    {
        //callback
        {
            DLog.LogMSG("LobbyManager / ProcessGlobalConstInfo / Callback");

            //서버에서 받을 내용 인위적으로 생성
            ErrorResult a_result = ErrorResult.OK;
            ServerGlobalValues serverValues = new ServerGlobalValues();
            serverValues.goldGambleCost = 1000;
            //

            Utility.ShowErrorCodeResult(a_result);

            if (a_result == ErrorResult.OK)
            {
                GlobalValues.Setup(serverValues);
                _loginProcessor.NextProcess();
            }
            else
            {
                _loginProcessor.FailProcess();
            }
            return;
        }
        
        //request 하는 부분 필요
        
    }
    private void ProcessNoticeBannerList()
    {
        //callback
        {
            DLog.LogMSG("LobbyManager / ProcessNoticeBannerList / Callback");

            //서버에서 받는것 인위적으로 만들기
            Dictionary<int, string> bannerList = new Dictionary<int, string>();

            //
            
            string nationName = GData.Instance.GetNationName(NativeBridge.LocaleData.code);

            Dictionary<int, string>.Enumerator itor = bannerList.GetEnumerator();
            while ( itor.MoveNext() )
            {
                string playerPrefsKey = PlayerPrefsManager.Instance.GetNoticeBannerURLKey(itor.Current.Key);
                if(PlayerPrefsManager.Instance.CompareValue(playerPrefsKey, System.DateTime.Now.ToString("yyyymmdd")))
                {
                    string url = itor.Current.Value;
                    if (url.Contains("/EN/"))//서버에서 받는 기본 값은 EN이다 이것을 언어에 맞게 고친다.
                    {
                        url = url.Replace("/EN/", "/" + NativeBridge.LocaleData.code + "/");
                    }

                    KeyValuePair<string, string> value = new KeyValuePair<string, string>(playerPrefsKey, url);
                    GInfo.noticeBannerList.Add(value);
                }
            }

            GInfo.noticeBannerList.Sort( (a, b) => 
            {
                if( a.Key.CompareTo(b.Key) > 0 )
                {
                    return -1;
                }
                else if(a.Key.CompareTo(b.Key) < 0)
                {
                    return 1;
                }
                return 0;
            });

            _loginProcessor.NextProcess();

        }

        //request
        
    }
    



    public void StartLoginProcess(Callback<bool> callback)
    {
        _loginProcessor.Start(callback);
    }











    

    private void Update()
    {

    }

    

    private void OnLeaveServer()
    {
    }

    public bool Connecting(string address, int port)
    {
        //서버 붙이면 추가

        return true;
    }
    public void Disconnect()
    {
        
    }

    private void ReceiveNotice(NoticeType type)
    {
        //GInfo.IncludeNoticeType();
        Messenger.Broadcast(EventKey.UpdateNotice, MessengerMode.DONT_REQUIRE_LISTENER);
    }



    public void RefreshEnergyReq()
    {
        //response
        {
            //서버에서 받을 부분 인위적 생성
            ErrorResult aResult = ErrorResult.OK;
            AssetInfo energyInfo = new AssetInfo();
            energyInfo.type = AssetType.Energy;
            energyInfo.nextChageTime = GInfo.serverTime.AddSeconds(5.0f);
            energyInfo.curCount = GInfo.GetAssetInfo(AssetType.Energy).curCount + 1;


            //DLog.LogMSG("RefreshEnergy");

            Utility.ShowErrorCodeResult(aResult);
            if(aResult != ErrorResult.OK)
            {
                return;
            }

            GInfo.UpdateAsset(energyInfo);

            Messenger.Broadcast(EventKey.UpdateAssetInfo, MessengerMode.DONT_REQUIRE_LISTENER);

        }


        //req

    }









}
