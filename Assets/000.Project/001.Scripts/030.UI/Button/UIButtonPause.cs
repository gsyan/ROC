using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonPause : MonoBehaviour
{
    private string resumeText;
    private string exitText;
    private Callback onExit;


    private void OnEnable()
    {
        Messenger.AddListener(EventKey.ShowPause, OnPause);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(EventKey.ShowPause, OnPause);
    }

    public void Setup(string resumeText, string exitText, Callback onExit)
    {
        this.resumeText = resumeText;
        this.exitText = exitText;
        this.onExit = onExit;
    }

    public void OnPause()
    {
        if (!Loading.isLoading && ScreenBlinder.Instance.isScreenVisible)
        {
            if (string.IsNullOrEmpty(resumeText))
            {
                resumeText = Localization.Get("resume");
            }

            if (string.IsNullOrEmpty(exitText))
            {
                exitText = Localization.Get("exit");
            }

            Transform tm = BKST.UISystem.Instance.ShowPanel("Panel Pause");
            if(tm != null)
            {
                tm.GetComponent<UIPanelPause>().Setup(resumeText, exitText, onExit);
            }
        }
    }


}
