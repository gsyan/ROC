using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPanelPause : UIPanelBase
{
    public Text resume;
    public Text exit;

    private Callback _exitCallback;

    public void Setup(string resumeText, string exitText, Callback exitCallback)
    {
        _exitCallback = exitCallback;
        resume.text = resumeText;
        exit.text = exitText;
    }

    public override void OnActive()
    {
        base.OnActive();
        GameTime.Pause();
    }

    public void Resume()
    {
        GameTime.Resume();
        BKST.UISystem.Instance.HidePanel(transform, false);
    }

    public void Exit()
    {
        GameTime.Resume();
        GInfo.isContinueBattle = false;

        if(_exitCallback != null)
        {
            _exitCallback();
            _exitCallback = null;
            BKST.UISystem.Instance.HidePanel(transform, false);
        }
        else
        {
            BKST.UISystem.Instance.SetInputState(false);
            StartCoroutine(LoadingLevel());
        }
    }

    private IEnumerator LoadingLevel()
    {
        yield return ScreenBlinder.Instance.BlinderFadeIn();
        SceneManager.LoadScene("Loading");
    }




}
