using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using UnityEngine.SceneManagement;//SceneManager



public class Login : MonoBehaviour
{
    //public bool showPrologue;
    //public bool showUITutorial;
    private GameEscape _escape = new GameEscape();



    private void Start ()
    {
#if UNITY_EDITOR
        //TheFirst 에서 수행
        NativeBridge.Instance.Init();
        GameSettings.Instance.Apply();
        //TheFirst 에서 수행
#endif
        //뒤로가기 기능
        _escape.Initialize(this, GameEscape.EscapeType.Login);

        //csv 정보 로드
        //이용 약관
        PlayerPrefs.SetString("roc_using_policy_url", "http://plus.934.co.kr/G.A/TermsOfUse");//url csv 화 필요
        //개인 정보
        PlayerPrefs.SetString("roc_private_policy_url", "http://plus.934.co.kr/G.A/privacy");//url csv 화 필요

        //구매 기능
        //PurchaserHelper.Instance.Init();

        //로그인 프리팹 로드
        ShowPanelLogin();

        //SocialLink.SocialInitialize();
    }
    
    private void ShowPanelLogin()
    {
        Transform panelLogin = UISystem.Instance.ShowPanel("Panel Login");
        if(panelLogin != null)
        {
            panelLogin.GetComponent<UIPanelLogin>().Setup();
        }



    }




}

