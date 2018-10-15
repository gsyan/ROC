using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleDefault : UIToggleExtension
{
    [HideInInspector]
    public Image image;

    [HideInInspector]
    public UILabel labelText;

    [HideInInspector]
    public UITweener tween;

    public List<EventDelegate> onUpdateState;



    public virtual void Awake()
    {
        image = GetComponentInChildren<Image>();
        labelText = GetComponentInChildren<UILabel>();
        tween = GetComponent<UITweener>();
    }


    public override void UpdateState(bool state)
    {
        if (state)
        {
            if (image != null)
            {
                image.color = new Color(1f, 1f, 1f, 1f);
            }

            if (labelText != null)
            {
                labelText.color = new Color(1f, 1f, 1f, 1f);
            }

            if (tween != null)
            {
                tween.PlayForward();
            }

            EventDelegate.Execute(onUpdateState);
        }
        else
        {
            if (image != null)
            {
                image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

            if (labelText != null)
            {
                labelText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

            if (tween != null)
            {
                tween.PlayReverse();
            }
        }
    }
}
