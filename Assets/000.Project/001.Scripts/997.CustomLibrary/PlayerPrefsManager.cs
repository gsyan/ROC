using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager
{
    private static PlayerPrefsManager _instance;
    public static PlayerPrefsManager Instance
    {
        get
        {
            if( _instance == null)
            {
                _instance = new PlayerPrefsManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 최근 로그인시 선택한 서버 저장
    /// </summary>
    public int Last_Selected_Server
    {
        set
        {
            PlayerPrefs.SetInt("last_selected_server", value);
        }
        get
        {
            return PlayerPrefs.GetInt("last_selected_server", 0);
        }
    }

    public string App_Version
    {
        set
        {
            PlayerPrefs.SetString("app_version", value);
        }
        get
        {
            return PlayerPrefs.GetString("app_version", Application.version);
        }
    }



    /// <summary>
    /// "NOTICE_BANNER_ID_{id}" 형태의 문자열 만들어 줌
    /// </summary>
    /// <param name="id">서버에서 받은 배너 url 의 숫자 key 값</param>
    /// <returns></returns>
    public string GetNoticeBannerURLKey(int id)
    {//playerprefs 에 해당 포맷으로 당일 안볼건지 저장, 저장 방법은 당일 날짜(System.DateTime.Now.ToString("yyyyMMdd"))를 저장한다. 로딩시 날짜가 같으면 당일 안보여주는 것으로 처리
        return string.Format("NOTICE_BANNER_ID_{0}", id);
    }

    public bool CompareValue(string key, string compareTarget)
    {
        if( PlayerPrefs.GetString(key).Equals(compareTarget) )
        {
            return true;
        }
        
        return false;
    }


}
