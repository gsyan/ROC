using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public enum HudScaleOption
{
    Normal,
    Small,
}
public class GameSettings : MonoBehaviour
{
    private static GameSettings _instance = null;
    public static GameSettings Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(GameSettings)) as GameSettings;
                if(_instance == null)
                {
                    GameObject obj = new GameObject("_GameSettings");
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<GameSettings>();
                }
            }
            return _instance;
        }
    }
    
    //  sound   ===============================================================================================
    public class SoundGroup//master, bgm, fx 인지를 구분하는 목적
    {
        public AudioMixerGroup group;
        public float factor;
    }

    private AudioMixer audioMixer;
    private Dictionary<string, SoundGroup> soundGroupDic = new Dictionary<string, SoundGroup>();

    private readonly float minValue = -80.0f;
    private readonly float maxValue = 0.0f;

    public bool isMute
    {
        get
        {
            return GetVolumeFactor("master") <= 0;
        }
    }
    public float GetVolumeFactor(string nameKey)
    {
        if (soundGroupDic.ContainsKey(nameKey))
        {
            //참고로 set은 soundGroupDic, audioMixer, PlayerPrefs 세개에 적용
            return soundGroupDic[nameKey].factor;
        }
        return 0.0f;
    }

    //Volume 을 Factor 를 이용해서 세팅
    public bool SetVolumeFactor(string nameKey, float factor)
    {
        if (soundGroupDic.ContainsKey(nameKey))
        {
            //참고로 get을 할때는 soundGroupDic 의 값을 가져다 씀, audioMixer 은 적용, PlayerPrefs 앱 최초 시작시 사용
            soundGroupDic[nameKey].factor = factor;
            //사운드의 급격한 변화를 방지
            //float mValue = factor * 0.5f + 0.5f;
            //if (mValue <= 0.5f) { mValue = 0.0f; }
            //factor 는 0 ~ 1 사이 이고, volume 은 -80 ~ 0 까지인 듯 한다.
            audioMixer.SetFloat(nameKey + "Volume", FactorToVolume(factor));//mValue
            PlayerPrefs.SetFloat("roc_sound_" + nameKey, factor);
            return true;
        }
        return false;
    }
    //Volume 을 세팅
    public bool SetVolume(string nameKey, float volume)
    {
        if (soundGroupDic.ContainsKey(nameKey))
        {
            soundGroupDic[nameKey].factor = VolumeToFactor(volume);
            audioMixer.SetFloat(nameKey + "Volume", volume);
            PlayerPrefs.SetFloat("roc_sound_" + nameKey, VolumeToFactor(volume));
            return true;
        }
        return false;
    }

    //factor(0~1)을 Volume으로
    float FactorToVolume(float factor)
    {
        return Mathf.Lerp(minValue, maxValue, factor);
    }
    // Volume을 factor(0~1)로
    float VolumeToFactor(float volume)
    {
        return Mathf.InverseLerp(minValue, maxValue, volume);
    }
    
    public AudioMixerGroup GetAudioMixerGroup(string nameKey)
    {
        SoundGroup soundGroup;
        if( soundGroupDic.TryGetValue(nameKey, out soundGroup) )
        {
            return soundGroup.group;
        }

        return null;
    }

    public void OnMute(bool state)
    {
        if (state)
        {
            SetVolume("master", minValue);
        }
        else
        {
            SetVolume("master", maxValue);
        }
    }

    




    //인스턴스가 생기면서 awake 에서 호출
    private void InitializeSound()
    {
        audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        if (audioMixer != null)
        {
            AudioMixerGroup[] soundGroups = audioMixer.FindMatchingGroups(string.Empty);//사운드 그룹을 모두다 가져옴
            
            for (int i=0; i< soundGroups.Length; ++i)
            {
                string nameKey = soundGroups[i].name.ToLower();
                float factor = PlayerPrefs.GetFloat("roc_sound_" + nameKey, 1.0f);

                SoundGroup soundGroup = new SoundGroup();
                soundGroup.factor = factor;
                soundGroup.group = soundGroups[i];
                
                soundGroupDic.Add(nameKey, soundGroup);
            }
        }
    }

    //Awake 함수 완료 후 호출
    private void ApplySound()
    {
        IEnumerator enumerator = soundGroupDic.Keys.GetEnumerator();
        while( enumerator.MoveNext() )
        {
            string key = (string)enumerator.Current;

            SoundGroup soundGroup;
            if (soundGroupDic.TryGetValue(key, out soundGroup))
            {
                SetVolumeFactor(key, soundGroup.factor);
            }
        }
    }




    //  resolution  ===============================================================================================
    private Vector2 baseScreenSize = Vector2.zero;
    private ScreenOrientation baseScreenOrientation = ScreenOrientation.Portrait;
    private HudScaleOption baseHudScale = HudScaleOption.Normal;

    private float screenRate = 1.0f;
    public readonly float screenRateLow = 0.625f;
    public readonly float screenRateMiddle = 0.75f;
    public readonly float screenRateHigh = 1;
    
    private void InitializeScreen()
    {
        screenRate = Mathf.Clamp01(PlayerPrefs.GetFloat("roc_base_screen_rate", 1.0f));
        baseScreenOrientation = (ScreenOrientation)(PlayerPrefs.GetInt("roc_base_screen_orientation", 0));
        baseHudScale = (HudScaleOption)PlayerPrefs.GetInt("roc_base_hud_scale", 0);
    }

    //Awake 함수 완료 후 호출
    private void ApplyScreen()
    {
        SetScreenOrientation(baseScreenOrientation);
        SetHudScale(baseHudScale);
    }

    public void SetScreenOrientation(ScreenOrientation orientation)
    {
        PlayerPrefs.SetInt("roc_base_screen_orientation", (int)orientation);

        if (orientation == ScreenOrientation.AutoRotation)
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
        }

        Screen.orientation = orientation;
        baseScreenOrientation = orientation;

        SetScreenSize(screenRate);
    }
    public void SetScreenSize(float rate)
    {
        screenRate = Mathf.Clamp01(rate);

        PlayerPrefs.SetFloat("roc_base_screen_rate", screenRate);

        switch (Screen.orientation)
        {
            case ScreenOrientation.Portrait:
                Screen.SetResolution((int)(baseScreenSize.y * screenRate), (int)(baseScreenSize.x * screenRate), Screen.fullScreen);
                break;

            case ScreenOrientation.Landscape:
                Screen.SetResolution((int)(baseScreenSize.x * screenRate), (int)(baseScreenSize.y * screenRate), Screen.fullScreen);
                break;
        }
    }

    public void SetHudScale(HudScaleOption scale)
    {
        PlayerPrefs.SetInt("roc_base_hud_scale", (int)scale);
        baseHudScale = scale;
    }

    public int GetCurrentScreenRateIndex()
    {
        return GetCurrentScreenRateIndex(screenRate);
    }
    public int GetCurrentScreenRateIndex(float screenRate)
    {
        if (Mathf.Abs(screenRate - screenRateHigh) < float.Epsilon)
        {
            return 0;
        }

        if (Mathf.Abs(screenRate - screenRateMiddle) < float.Epsilon)
        {
            return 1;
        }

        if (Mathf.Abs(screenRate - screenRateLow) < float.Epsilon)
        {
            return 2;
        }

        return 0;
    }

    public int GetCurrentScreenOrientationIndex()
    {
        switch (baseScreenOrientation)
        {
            case ScreenOrientation.Portrait:
                return 0;

            case ScreenOrientation.Landscape:
                return 1;

            case ScreenOrientation.AutoRotation:
                return 2;
        }

        return 0;
    }

    public int GetCurrentHudScaleIndex()
    {
        return (int)baseHudScale;
    }

    public int GetCurrentFrameIndex()
    {
        return GetFrameIndex(currentFrame);
    }

    public HudScaleOption GetHudScaleOption()
    {
        return baseHudScale;
    }
    
    public float GetHudScale()
    {
        if (Screen.width > Screen.height)
        {
            if (baseHudScale == HudScaleOption.Normal)
            {
                return 1.3f;
            }
            else
            {
                return 1.0f;
            }
        }
        // Portrait
        else
        {
            if (baseHudScale == HudScaleOption.Normal)
            {
                return 2.9f; // 패널의 스케일을 변경. 2017.08.02-zin
                //return 1.0f; 
            }
            else
            {
                return 2.7f; // 패널의 스케일을 변경. 2017.08.02-zin
                //return 0.8f; 
            }
        }
    }






    //  Frame  ===============================================================================================
    private int currentFrame;
    public readonly int frame24 = 24;
    public readonly int frame30 = 30;
    public readonly int frame60 = 60;

    void InitializeFrame()
    {
        currentFrame = PlayerPrefs.GetInt("roc_frame", frame60);
    }

    void ApplyFrame()
    {
        SetFrame(currentFrame);
    }

    public void SetFrame(int frame)
    {
        PlayerPrefs.SetInt("roc_frame", frame);
        currentFrame = frame;
        Application.targetFrameRate = currentFrame;
    }

    public int GetFrameIndex(int frame)
    {
        switch (frame)
        {
            case 60: return 0;
            case 30: return 1;
            case 24: return 2;
        }

        return 0;
    }


    //  method  ===============================================================================================
    private void Awake()
    {
        int bigger = 0;
        int shorter = 0;
        if (Screen.width > Screen.height)
        {
            bigger = Screen.width;
            shorter = Screen.height;
        }
        else
        {
            bigger = Screen.height;
            shorter = Screen.width;
        }
        baseScreenSize.x = bigger;
        baseScreenSize.y = shorter;


        InitializeSound();
        InitializeScreen();
        InitializeFrame();
    }

    private void OnDestroy()
    {
        soundGroupDic.Clear();
        _instance = null;
    }

    //Awake 함수 완료 후 호출
    public void Apply()
    {
        ApplySound();
        ApplyScreen();
        ApplyFrame();
    }

    















}
