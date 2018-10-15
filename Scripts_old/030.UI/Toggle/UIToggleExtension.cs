using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIToggleExtension : UIWidgetContainer
{
    /// <summary>
    /// List of all the active toggles currently in the scene.
    /// </summary>

    static public BetterList<UIToggleExtension> list = new BetterList<UIToggleExtension>();

    /// <summary>
    /// Current toggle that sent a state change notification.
    /// </summary>

    static public UIToggleExtension current;

    /// <summary>
    /// If set to anything other than '0', all active toggles in this group will behave as radio buttons.
    /// </summary>

    public int group = 0;

    /// <summary>
    /// Whether the toggle starts checked.
    /// </summary>

    public bool enableCheck = false;   //bk extention, check active tab when enabled
    public bool startsActive = false;
    
    /// <summary>
    /// Can the radio button option be 'none'?
    /// </summary>

    public bool optionCanBeNone = false;

    /// <summary>
    /// Callbacks triggered when the toggle's state changes.
    /// </summary>

    public List<EventDelegate> onChange = new List<EventDelegate>();

    public delegate bool Validate(bool choice);

    /// <summary>
    /// Want to validate the choice before committing the changes? Set this delegate.
    /// </summary>

    public Validate validator;

    /// <summary>
    /// Deprecated functionality. Use the 'group' option instead.
    /// </summary>

    [HideInInspector]
    [SerializeField]
    GameObject eventReceiver;
    [HideInInspector]
    [SerializeField]
    string functionName = "OnActivate";
    [HideInInspector]
    [SerializeField]
    bool startsChecked = false; // Use 'startsActive' instead

    bool mIsActive = true;
    bool mStarted = false;

    /// <summary>
    /// Whether the toggle is checked.
    /// </summary>

    public bool value
    {
        get
        {
            return mStarted ? mIsActive : startsActive;
        }
        set
        {
            if (!mStarted) startsActive = value;
            else if (group == 0 || value || optionCanBeNone || !mStarted) Set(value);
        }
    }

    /// <summary>
    /// Whether the collider is enabled and the widget can be interacted with.
    /// </summary>

    public bool isColliderEnabled
    {
        get
        {
            Collider c = GetComponent<Collider>();
            if (c != null) return c.enabled;
            Collider2D b = GetComponent<Collider2D>();
            return (b != null && b.enabled);
        }
    }

    [System.Obsolete("Use 'value' instead")]
    public bool isChecked
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
        }
    }

    /// <summary>
    /// Return the first active toggle within the specified group.
    /// </summary>

    static public UIToggleExtension GetActiveToggle(int group)
    {
        for (int i = 0; i < list.size; ++i)
        {
            UIToggleExtension toggle = list[i];
            if (toggle != null && toggle.group == group && toggle.mIsActive)
            {
                return toggle;
            }
        }

        return null;
    }

    void OnEnable()
    {
        list.Add(this);
        
        //bk extention
        if (enableCheck)
        {
            Set(startsActive);
        }
        //
    }

    void OnDisable()
    {
        list.Remove(this);
    }

    /// <summary>
    /// Activate the initial state.
    /// </summary>

    void Start()
    {
        if (startsChecked)
        {
            startsChecked = false;
            startsActive = true;
#if UNITY_EDITOR
            NGUITools.SetDirty(this);
#endif
        }

        // Auto-upgrade
        if (!Application.isPlaying)
        {
            if (EventDelegate.IsValid(onChange))
            {
                eventReceiver = null;
                functionName = null;
            }
        }
        else
        {
            mIsActive = !startsActive;
            mStarted = true;
            Set(startsActive);
        }
    }

    /// <summary>
    /// Check or uncheck on click.
    /// </summary>

    void OnClick()
    {
        if (enabled)
        {
            value = !value;
        }
    }

    /// <summary>
    /// Fade out or fade in the active sprite and notify the OnChange event listener.
    /// </summary>

    public void Set(bool state)
    {
        if (validator != null && !validator(state)) return;

        if (!mStarted)
        {
            mIsActive = state;
            startsActive = state;
        }
        else if (mIsActive != state)
        {
            // Uncheck all other toggles
            if (group != 0 && state)
            {
                for (int i = 0, imax = list.size; i < imax;)
                {
                    UIToggleExtension cb = list[i];
                    if (cb != this && cb.group == group) cb.Set(false);

                    if (list.size != imax)
                    {
                        imax = list.size;
                        i = 0;
                    }
                    else ++i;
                }
            }

            // Remember the state
            mIsActive = state;

            if (current == null)
            {
                UIToggleExtension tog = current;
                current = this;

                if (EventDelegate.IsValid(onChange))
                {
                    EventDelegate.Execute(onChange);
                }
                else if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
                {
                    // Legacy functionality support (for backwards compatibility)
                    eventReceiver.SendMessage(functionName, mIsActive, SendMessageOptions.DontRequireReceiver);
                }
                current = tog;
            }

            UpdateState(state);
        }
    }

    /// <summary> bk added, tag alarm and skill list bug fix
    /// 케릭터 테그시, 활성화된 탭이 기존 케릭터 스킬 정보로 구성되어있는데 이를 테그된 케릭 정보로 업데이트 하기 위해서는
    /// mIsActive 값이 false 여야 업데이트 코드로 들어가게 된다.
    /// </summary>

    public void SetIsActive(bool b)
    {
        mIsActive = b;
    }

    public virtual void UpdateState(bool state)
    {
    }
}
