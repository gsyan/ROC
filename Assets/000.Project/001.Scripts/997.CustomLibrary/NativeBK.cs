using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//JsonUtility 용이라면 public, SerializeField, 맴버들도 다 public 
[SerializeField]
public class LocaleData
{
    public string iso;
    public string code;
    public string name;
    public string lang;
}

public class NativeBK : MonoBehaviour
{
    private static NativeBK _instance;
    public static NativeBK Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<NativeBK>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("_NativeBK");
                    _instance = obj.AddComponent<NativeBK>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

#if UNITY_ANDROID
    static private AndroidJavaObject _activity = null;
#endif

    
    public void Init()
    {
#if UNITY_EDITOR

#elif UNITY_ANDROID
        _activity = new AndroidJavaObject("com.bkst.mainpluginandroid.MainActivity");
        GetGAIDFromNative();
        GetLocalDataFromNative();
#endif
    }

    public static void LogMSG(string str)
    {
#if UNITY_ANDROID
        _activity.Call("Log", str);
#endif

    }
    




    //구글 광고 아이디 관련
    private static string _gaid;
    public void GetGAIDFromNative()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        DLog.LogMSG("NativeBK / GetGAIDFromNative");
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        activity.CallStatic("GetGAID", gameObject.name, "SetGAID");
#endif
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
#elif UNITY_IOS
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


}
