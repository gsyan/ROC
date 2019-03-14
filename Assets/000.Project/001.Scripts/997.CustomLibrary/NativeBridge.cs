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


    static private AndroidJavaObject _activity = null;//currentActivity 용 변수

    public void Init()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        
        GetLocalDataFromNative();
#endif
    }

    public static void Log(string msg)
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        _activity.Call("LogMSG", msg);
#endif

    }



    //지역 관련
    #region Locale
    private static LocaleData _localeData;
    public void GetLocalDataFromNative()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.bkst.pluginbk.LocaleBK");
        jc.CallStatic("GetLocaleList");
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
        OpenNativeWebViewWithURL(url);
    }
#if UNITY_EDITOR
    public void OpenNativeWebViewWithURL(string url) { }
#elif UNITY_ANDROID
    public void OpenNativeWebViewWithURL(string url)
    {
        _activity.Call("OpenNativeWebView", new string[] { url });
    }
#elif UNITY_IPHONE
    [DllImport("__Internal")] public static extern void OpenNativeWebViewWithURL(string aParam);
#endif
    
    #endregion WebView


    #region Toast

    public void Toast(string meg)
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
		object[] parameters = new object[2];
        parameters[0] = _activity;
        parameters[1] = meg;
        _activity.Call("ToastString", parameters);
#elif UNITY_IPHONE

#endif

    }

    #endregion


}

