using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BKST/Tween/Tween Position")]
public class TweenPosition : UITweener
{
    public Vector2 from;
    public Vector2 to;

    public Vector2 value
    {
        get
        {
            return transform.GetComponent<RectTransform>().anchoredPosition;
        }
        set
        {
            transform.GetComponent<RectTransform>().anchoredPosition = value;
        }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        //throw new System.NotImplementedException();
        value = from * (1.0f - factor) + to * factor;
    }

    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if(duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }

}
