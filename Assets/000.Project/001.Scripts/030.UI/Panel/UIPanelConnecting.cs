using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelConnecting : UIPanelBase
{
    public bool autoDisable = true;
    public float disableCount = 10.0f;

    private void OnEnable()
    {
        if (autoDisable)
        {
            StartCoroutine(AutoDisable());
        }
    }

    private IEnumerator AutoDisable()
    {
        float time = 0.0f;
        while(time < disableCount )
        {
            time += GameTime.deltaTime;
            yield return null;
        }
        BKST.UISystem.Instance.HidePanel(transform, false);
    }
}
