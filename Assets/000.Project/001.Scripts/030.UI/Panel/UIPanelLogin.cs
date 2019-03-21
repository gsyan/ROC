using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;


public class UIPanelLogin : UIPanelBase
{
    public Dropdown serverDropDown;

    public GameObject buttonConnect;
    public Text textState;

    public GameObject loginButtonRoot;
    private GameObject _loginButtonSocial;
    private GameObject _loginButtonPlatform;
    private GameObject _loginButtonGuest;

    private List<ServerData> _serverlist = new List<ServerData>();
    private string _address = "";   //_serverlist 의 특정 인덱스의 ServerData.address 값 대입
    private int _port = 0;          //_serverlist 의 특정 인덱스의 ServerData.port 값 대입

    private List<string> _userIdList;
    
    



    public override void Awake()
    {
        base.Awake();

        if (loginButtonRoot != null)
        {
            _loginButtonSocial = loginButtonRoot.transform.Find("Social").gameObject;
            _loginButtonPlatform = loginButtonRoot.transform.Find("Platform").gameObject;
            _loginButtonGuest = loginButtonRoot.transform.Find("Guest").gameObject;
        }

        _customGuiSkin = Resources.Load("ETC/GaGUISkin") as GUISkin;
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Start()
    {
        DeactiveAllLoginButton();
    }
    private void DeactiveAllLoginButton()
    {
        _loginButtonSocial.SetActive(false);
        _loginButtonPlatform.SetActive(false);
        _loginButtonGuest.SetActive(false);
    }

    public void Setup()
    {
        //ServerList.csv 내용 리턴
        _serverlist = GData.Instance.GetServerList();
        if (_serverlist.Count <= 0) { return; }

        //서버선택 ui 세팅
        serverDropDown.options.Clear();
        for (int i=0; i<_serverlist.Count; ++i)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = Localization.Get(_serverlist[i].name);
            serverDropDown.options.Add(od);
        }

        

        //현재 선택된 서버 ui 세팅
        int index = PlayerPrefsManager.Instance.Last_Selected_Server;

#if GAMESERVER_QA
        index = 3;//Work = 0, Dev = 1, test = 2, QA = 3, live = 4,
        PlayerPrefsManager.Instance.Last_Selected_Server = 3;
#elif GAMESERVER_LIVE
        index = 4;//Work = 0, Dev = 1, test = 2, QA = 3, live = 4,
        PlayerPrefsManager.Instance.Last_Selected_Server = 4;
#endif
        //오류 바로잡기
        if (index >= _serverlist.Count)
        {
            index = 0;
            PlayerPrefsManager.Instance.Last_Selected_Server = 0;
        }

        serverDropDown.value = index;
        
        _address = _serverlist[index].address;
        _port = _serverlist[index].port;
        DLog.LogMSG("server: " + _serverlist[index].name + "// address: " + _address + "// port: " + _port);
        
#if GAMESERVER_QA || GAMESERVER_LIVE// server 한정 버전의 경우 자동으로 TryConnect()
        TryConnect();
#endif
    }

    public void OnSelectServer()//서버 선택 완료 모드로 전환
    {
        PlayerPrefsManager.Instance.Last_Selected_Server = serverDropDown.value;
        _address = _serverlist[serverDropDown.value].address;
        _port = _serverlist[serverDropDown.value].port;
        
        buttonConnect.SetActive(true);
    }



    //Panel Login 의 Button Connect 를 누르면 call    
    public void TryConnect()
    {
        DLog.LogMSG("TryConnect");

        textState.text = Localization.Get("connecting_server");
        buttonConnect.SetActive(false);
        serverDropDown.gameObject.SetActive(false);
        
        //server connecting and complete then call delegate
        {
            OnJoinLobbyServerComplete();
        }

    }

    private void OnJoinLobbyServerComplete()
    {
        if(true)//접속이 잘 되었다면
        {
            textState.text = "";
#if UNITY_EDITOR
            UnityEditorCase();
#elif UNITY_STANDALONE
            ShowLoginButton();
#elif LOGIN_FACEBOOK
            FaceBookCase();
#elif LOGIN_KAKAO

#endif
        }
    }
    private void UnityEditorCase()
    {
        UserIdFile.Read(out _userIdList);
        if (_userIdList.Count == 0)
        {
            ShowLoginButtonForUnity();
        }
        else
        {
            
        }
    }
    private void FaceBookCase()
    {

    }
    private void ShowLoginButtonForUnity()
    {
        DeactiveAllLoginButton();
        _loginButtonGuest.SetActive(true);
    }


    public void LoginSocialButton()
    {

    }
    public void LoginPlatformButton()
    {

    }
    public void LoginGuestButton()
    {
        DLog.LogMSG("LoginGuestButtonClick");
        Utility.ShowMessageBox(Localization.Get("login_quest"), Localization.Get("login_quest_message"), MessageBoxType.YesNo,
            delegate ()
            {
                DLog.LogMSG("LoginGuest OK");
                loginButtonRoot.SetActive(false);

                List<string> list;
                UserIdFile.ReadPrev(out list);
                if( list.Count > 0)
                {
                    //기존의 데이터 있던것으로 쓸지 말지 가려서 처리
                    Utility.ShowMessageBox(Localization.Get("prev_guest_id"), Localization.Get("prev_guest_id_message"), MessageBoxType.OkCancel,
                        delegate ()//ok use prev
                        {
                            UserIdFile.DeletePrev();
                            UserIdFile.Write(list[0]);
                            Login(list[0]);
                        },
                        delegate ()//no make new id
                        {
                            UserIdFile.DeletePrev();
                            StartCoroutine(LoadingScene("NewStart"));
                        },
                        delegate ()//no I wish see login buttons again
                        {
                            loginButtonRoot.SetActive(true);
                        });   
                }
                else
                {
                    //첫 게스트 로그인으로 진행
                    StartCoroutine(LoadingScene("NewStart"));
                }
            }
        );


    }
    








    private void Login(string userId)
    {
        textState.text = Localization.Get("try_login");

        //로그인 완료 콜백
        {
            //서버에서 받을 정보 인위적으로 만듬
            ErrorResult a_result = ErrorResult.OK;

            GInfo.SetupTest();//테스토용으로 자료를 만든다.
            PlayerInfo playerInfo = GInfo.playerInfo;//서버 대신 태스트 코드에서 정보 받음
            DateTime serverTime = DateTime.Now;//서버 시간 대신 현재 시간 받음

            BlockedAccountInfo blockInfo = new BlockedAccountInfo();//blockInfo 테스트

            //서버에서 받을 정보 인위적으로 만듬 end


            if (a_result == ErrorResult.ExistUser)//이미 접속된 유저
            {
                StartCoroutine(RetryLogin());
            }
            else if ( a_result == ErrorResult.BlockedAccount)//블럭된 유저 등 가려내기
            {
                BlockedAccountProcess(blockInfo);
            }
            else if( a_result == ErrorResult.NotFoundUser)
            {
                _userIdList.Remove(userId);//해당 유저정보 지우고
                UserIdFile.Write(_userIdList);//파일 내용에 반영
            }
            else if( a_result == ErrorResult.OK)
            {
                GInfo.serverTimeAtLogin = serverTime;
                GInfo.Setup(playerInfo);

                Utility.ShowConnecting();
                LobbyManager.Instance.StartLoginProcess(delegate (bool isSuccess)
                {
                    if( isSuccess )
                    {
                        SceneLoad.spawnPoint = "Spawn Default";
                        StartCoroutine(LoadingScene("CampYakSan"));
                    }
                    else
                    {
                        DLog.LogMSG("======== Failed At StartLoginProcess() ========");
                    }
                });

            }
            else
            {
                textState.text = "Login Failed, UnKnown a_result";
            }

            


            if(a_result == ErrorResult.OK)
            {
                


                LobbyManager.Instance.StartLoginProcess(delegate(bool result)
                {
                    if( result )
                    {
                        SceneLoad.spawnPoint = "Spawn Default";
                        StartCoroutine(LoadingScene("CampYakSan"));
                    }
                    else
                    {
                        DLog.LogMSG("Failed at LoginProcess");
                    }
                });



            }
            else if(a_result == ErrorResult.NotFoundUser)
            {
                _userIdList.Remove(userId);
                UserIdFile.Write(_userIdList);
            }
            else
            {
                textState.text = "Login Failed, UIPanelLogin / Login()";
            }
        }
        
        DLog.LogMSG(string.Format("Login userId = {0} / deviceUniqueIdentifier = {1}", userId, SystemInfo.deviceUniqueIdentifier));
        
        //로그인 시도
        //LobbyManager.Instance.

    }

    /// <summary>
    /// loading scene 을 띄우고 그 다음 sceneName scene 을 로드
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadingScene(string sceneName)
    {
        yield return ScreenBlinder.Instance.BlinderFadeIn();

        SceneLoad.nextScene = sceneName;
        SceneManager.LoadScene("Loading");

    }

    private int retryLoginCount = 0;
    IEnumerator RetryLogin()
    {
        if(retryLoginCount < 10)
        {
            yield return new WaitForSeconds(2.0f);
            if(_userIdList.Count > 0)
            {
                Login(_userIdList[0]);
            }
            else
            {
                Login("");
            }

            retryLoginCount++;
        }
        else
        {
            textState.text = Localization.Get("failed_Login_at : RetryLogin()");
            Utility.HideConnecting();
        }
    }

    private void BlockedAccountProcess(BlockedAccountInfo blockInfo)
    {
        string comment2 = "";
        //bk 작성 & 주석, 다음에 필요하면 쓰자
        //string AorP = "AM";
        //int showHour = blockInfo.endTime.Hour;
        //if (blockInfo.status == BlockStatus.Period)
        //{
        //    if (showHour > 11)
        //    {
        //        AorP = "PM";
        //        if (showHour > 12)
        //        {
        //            showHour -= 12;
        //        }
        //    }

        //    comment2 = string.Format("\n({0} {1}/ {2}/ {3}. {4} {5})",
        //            "UNTIL: ",
        //            blockInfo.endTime.Year,
        //            blockInfo.endTime.Month.ToString("00"),
        //            blockInfo.endTime.Day.ToString("00"),
        //            showHour.ToString("00"),
        //            AorP);
        //}

        string comment = string.Format("{0}{1}", blockInfo.comment, comment2);

        Utility.ShowMessageBox(Localization.Get("block_title"), comment, MessageBoxType.Ok, delegate () {
            Application.Quit();
        });
    }





    private GUISkin _customGuiSkin;
    private Vector2 _scrollPosition;
    private void OnGUI()
    {
        //if (!showGUI) return;

        //int width = (int)(Screen.width * 0.8f);
        //int height = (int)(Screen.height * 0.1f);
        //int totalHeight = height * Math.Min(_userIdList.Count + 1, 6);
        //int x = (Screen.width - width) / 2;
        //int y = (Screen.height - totalHeight) / 2;

        //_scrollPosition = GUI.BeginScrollView(  new Rect(x, y, width + 20, totalHeight),//스크롤 틀 위치와 크기
        //                                        _scrollPosition,//내부 스크롤 그룹의 시작 포지션( 스크롤이 되도록 하는 값 )
        //                                        new Rect(x, y, width, height * (_userIdList.Count + 1))//내부 스크롤 그룹의 크기
        //                                        );

        //if (_customGuiSkin != null) { GUI.skin = _customGuiSkin; }//bk: gui skin 

        //if (GUI.Button(new Rect(x, y, width / 2, height), "Create New ID"))
        //{
        //    showGUI = false;
        //    //StartCoroutine(LoadingLevel("Movie"));
        //}

        //if (GUI.Button(new Rect(x + width / 2, y, width / 2, height), "Delete All ID"))
        //{
        //    try
        //    {
        //        UserIdFile.Delete();
        //        _userIdList.Clear();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Log(e.ToString());
        //    }
        //}

        //for (int i = 0; i < _userIdList.Count; i++)
        //{
        //    if (GUI.Button(new Rect(x, y + i * height + height, width, height), _userIdList[i].ToString()))
        //    {
        //        Login(_userIdList[i]);
        //        showGUI = false;
        //    }
        //}
        
        //GUI.EndScrollView();


    }


}
