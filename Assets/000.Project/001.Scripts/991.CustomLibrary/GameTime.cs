using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Unity Time 클래스에 기능이 추가된 클래스. 게임내에서 사용함.
/// </summary>
public class GameTime : MonoBehaviour
{
    private static GameTime _instance;
    public static GameTime Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameTime>() as GameTime;
                if(_instance == null)
                {
                    GameObject go = new GameObject("_GameTime");
                    _instance = go.AddComponent<GameTime>();
                    DontDestroyOnLoad(go);
                }
            }

            return _instance;
        }
    }
    public void Init()
    {
        //just init function
    }

    public float lastTimeScale = 1.0f;
    public float targetTimeScale = 1.0f;
    public float amount = 1.0f;

    public bool isPause = false;
    public bool isFading = false;
    public bool isFinish = false;

    public static float timeScale
    {
        get
        {
            return Time.timeScale;
        }
        set
        {
            Time.timeScale = value;
        }
    }
    public static float deltaTime
    {
        get
        {
            return Time.deltaTime;
        }
    }
    //timescale 이 0일때도 증가하는 수치
    public static float unscaledDeltaTime
    {
        get
        {
            return _instance.isPause ? 0.0f : Time.unscaledDeltaTime;
        }
    }

    public static void Pause()
    {
        if(!_instance.isPause)
        {
            _instance.lastTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            _instance.isPause = true;
        }
    }
    public static void Resume()
    {
        if(_instance.isPause)
        {
            Time.timeScale = _instance.lastTimeScale;
            _instance.isPause = false;
        }
    }

    public static IEnumerator FinishBattle()
    {
        Time.timeScale = 0.3f;

        _instance.isFading = false;
        _instance.isFinish = true;

        float elapsedTime = 0.0f;
        while(true)
        {
            elapsedTime += unscaledDeltaTime;
            if(elapsedTime >= 2.5f)
            {
                break;
            }
            yield return null;
        }
        Time.timeScale = 1.0f;
        _instance.isFinish = false;
    }

    public static void FadeTimeScale(float targetTimeScale, float amount)
    {
        _instance.targetTimeScale = targetTimeScale;
        _instance.amount = amount;
        _instance.isFading = true;
    }

    public static void ResetTimeScale()
    {
        if (_instance.isPause) return;
        if (_instance.isFinish) return;

        Time.timeScale = 1.0f;
        _instance.isFading = false;
    }

    private void Update()
    {
        if (isPause || isFinish) return;
        
        if( isFading )
        {
            if(targetTimeScale > Time.timeScale)
            {
                float nextValue = Time.timeScale + amount * Time.unscaledDeltaTime;
                if(nextValue > targetTimeScale)
                {
                    Time.timeScale = targetTimeScale;
                    isFading = false;
                }
                else
                {
                    Time.timeScale = nextValue;
                }
            }
            else
            {
                float nextValue = Time.timeScale - amount * Time.unscaledDeltaTime;
                if(nextValue < targetTimeScale)
                {
                    Time.timeScale = targetTimeScale;
                    isFading = false;
                }
                else
                {
                    Time.timeScale = nextValue;
                }
            }
        }
    }
    
    private void OnLevelWasLoaded(int level)
    {
        isPause = false;
        isFading = false;
        isFinish = false;

        Time.timeScale = 1.0f;
    }

    

}
