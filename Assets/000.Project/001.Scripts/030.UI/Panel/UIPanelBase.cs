using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PanelType
{
    None,
    Main,
    Sub,
    Popup,
    ToolTip,
    Max,
}
public enum PanelManageType
{
    None,
    DontDestory,
}


public class UIPanelBase : MonoBehaviour
{
    private List<UITweener> _tweeners = new List<UITweener>();
    private int _defaultGroup = 1;

    private Stack<UIPanelBase> _subPanelStack = new Stack<UIPanelBase>();

    public UITypeBK uiType = UITypeBK.Overlay;
    public int depth = 0;//0~9 클수록 앞쪽에 배치

    [HideInInspector] public Transform cachedTransform;
    public PanelType type = PanelType.None;
    public PanelManageType manageType = PanelManageType.None;

    public bool blockScreen = true;
    [HideInInspector] public bool isDeactiveCall = false;

    private Coroutine _DeactiveCoroutine = null;

    public virtual void Awake()
    {
        bool includeInactive = true;//비활성화 된 차일드 까지 포함하냐?
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>(includeInactive);
        if (audioSources != null)
        {
            for (int i = 0; i < audioSources.Length; ++i)
            {

                audioSources[i].outputAudioMixerGroup = GameSettings.Instance.GetAudioMixerGroup(SoundType.Fx.ToString().ToLower());
            }
        }

        UITweener[] tweens = GetComponentsInChildren<UITweener>();
        if (tweens != null)
        {
            for (int i = 0; i < tweens.Length; ++i)
            {
                if (tweens[i].tweenGroup == _defaultGroup)
                {
                    _tweeners.Add(tweens[i]);
                }
            }
        }

        cachedTransform = transform;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        OnActive();

        if (_DeactiveCoroutine != null)
        {
            StopCoroutine(_DeactiveCoroutine);
        }

        for (int i = 0; i < _tweeners.Count; ++i)
        {
            _tweeners[i].PlayForward();
        }
    }

    public void Hide(Callback callback = null, bool immediate = false)
    {
        while( _subPanelStack.Count > 0)
        {
            _subPanelStack.Pop().gameObject.SetActive(false);
        }

        if(_DeactiveCoroutine != null)
        {
            StopCoroutine(_DeactiveCoroutine);
            _DeactiveCoroutine = null;
        }

        if(immediate)
        {
            OnDeactive();
            gameObject.SetActive(false);
            if(callback != null)
            {
                callback();
            }
        }
        else
        {
            if(gameObject.activeSelf)
            {
                _DeactiveCoroutine = StartCoroutine(Deactive(callback));
            }
        }
    }

    IEnumerator Deactive(Callback callback)
    {
        //bk: 이 함수의 작동 방식 점검

        if(_tweeners.Count > 0)
        {
            for(int i = 0; i < _tweeners.Count; ++i)
            {
                _tweeners[i].PlayReverse();
            }

            while(true)
            {
                bool isLooping = false;
                for(int i = 0; i < _tweeners.Count; ++i)
                {
                    if(_tweeners[i].tweenFactor > 0.0f)
                    {
                        isLooping = true;
                        break;
                    }
                }

                if (!isLooping)
                {
                    break;
                }

                yield return null;
            }
        }

        OnDeactive();
        gameObject.SetActive(false);

        if (callback != null)
        {
            callback();
        }

        _DeactiveCoroutine = null;

        yield return null;
    }








    public void PushSubPanel(UIPanelBase panel)
    {
        _subPanelStack.Push(panel);
    }
    public UIPanelBase PopSubPanel()
    {
        if (_subPanelStack.Count > 0)
        {
            return _subPanelStack.Pop();
        }
        return null;
    }








    public virtual void OnActive()
    {
        isDeactiveCall = false;
    }
    public virtual void OnDeactive()
    {
        isDeactiveCall = true;
    }
}
