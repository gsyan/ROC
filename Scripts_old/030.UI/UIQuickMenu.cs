using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMenu : MonoBehaviour
{
    public UITweener tween;
    public UIToggle toggleButton;
    public UISprite toggleSprite;

    public GameObject body;
    
    public void OnToggle()
    {
        if (toggleButton.value)//메인 메뉴가 노출될 때
        {
            tween.PlayForward();
            toggleSprite.cachedTransform.localEulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
        }
        else//메인 메뉴가 숨겨질 때
        {
            tween.PlayReverse();
            toggleSprite.cachedTransform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
        }
    }


    public void SetToggle(bool value)
    {
        toggleButton.value = value;
    }
}
