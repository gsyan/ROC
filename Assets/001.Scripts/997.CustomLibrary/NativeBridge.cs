using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;// for DllImport


//JsonUtility 용이라면 public, SerializeField, 맴버들도 다 public 
[SerializeField]
public class LocaleData
{
    public string iso;
    public string code;
    public string name;
    public string lang;
}

public class NativeBridge : MonoBehaviour
{
    private static NativeBridge _instance;
    public static NativeBridge Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<NativeBridge>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("_NativeBridge");
                    _instance = obj.AddComponent<NativeBridge>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

#if UNITY_ANDROID
    static private AndroidJavaObject _activity = null;

    //web view 예제 변수 잘 되면 _activity 를 _currentActivity으로 대체한다
    private AndroidJavaClass _player;
    private AndroidJavaObject _currentActivity;
    /////
    

#endif

    
    public void Init()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        _activity = new AndroidJavaObject("com.bkst.mainpluginandroid.MainActivity");
        //GetGAIDFromNative();
        //GetLocalDataFromNative();
#endif
    }

    public static void LogMSG(string str)
    {
#if UNITY_ANDROID
        _activity.Call("Log", str);
#endif

    }





    //구글 광고 아이디 관련
    #region GAID
    private static string _gaid;
    public void GetGAIDFromNative()
    {
        DLog.LogMSG("NativeBridge / GetGAIDFromNative");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        activity.CallStatic("GetGAID", gameObject.name, "SetGAID");
    }
    public void SetGAID(string gaid)//네이티브에서 호출되는 함수
    {
        _gaid = gaid;
    }
    public static string GAID
    {
        get
        {
#if UNITY_EDITOR
            return "Unity_Editor_GAID";//유니티면 
#elif UNITY_ANDROID
            return _gaid;
#endif
        }
    }
    #endregion GAID

    //지역 관련
    #region Locale
    private static LocaleData _localeData;
    public void GetLocalDataFromNative()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        _activity.Call("GetLocalesList");
#elif UNITY_EDITOR

#endif
    }
    public void SetLocale(string json)//call from native
    {
        //Log("bkutil SetLocale() str:" + json);
        _localeData = JsonUtility.FromJson<LocaleData>(json);

        //Log("bkutil SetLocale() _localeData.iso:" + _localeData.iso);
        //Log("bkutil SetLocale() _localeData.code:" + _localeData.code);
        //Log("bkutil SetLocale() _localeData.name:" + _localeData.name);
        //Log("bkutil SetLocale() _localeData.lang:" + _localeData.lang);
    }
    public static LocaleData LocaleData
    {
        get
        {
#if UNITY_EDITOR
            _localeData = new LocaleData();
            _localeData.code = "KR";
            return _localeData;
#elif UNITY_ANDROID
			return _localeData;
#elif UNITY_IPHONE
			_localeData = new LocaleData();
			//임시
			_localeData.code = "KR";
			return _localeData;
            
#elif UNITY_STANDALONE
            _localeData = new LocaleData();
            _localeData.code = "pc";
            return _localeData = new LocaleData();
#endif
        }
    }
    #endregion Locale


    #region WebView

    public void OpenWebView(string url)
    {
        openNativeWebViewWithURL(url);
    }
#if UNITY_ANDROID
    public void openNativeWebViewWithURL(string url)
    {
        if(_player == null && _currentActivity == null)
        {
            _player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _currentActivity = _player.GetStatic<AndroidJavaObject>("currentActivity");
        }
        _currentActivity.Call("openNativeWebView", new string[] { url });
    }
#elif UNITY_IPHONE
    [DllImport("__Internal")] public static extern void openNativeWebViewWithURL(string aParam);
#else
    public void openNativeWebViewWithURL(string url) { }
#endif


    #endregion WebView


}
