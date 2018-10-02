using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static bool isLoading = false;//GameEscape 가능 상태 알아볼때 도 쓰임

    IEnumerator Start()
    {
        isLoading = true;

        BKST.UISystemBK.Instance.SetInputState(true);

        //ShowPanel
        BKST.UISystemBK.Instance.ShowPanel(GlobalValues.PANEL_LOADING);
        ScreenBlinder.Instance.BlinderOff();
        yield return null;

        ResourceSystem.UnloadUnusedReference();
        yield return null;

        Resources.UnloadUnusedAssets();
        yield return null;
        
        SceneManager.LoadScene(SceneLoad.nextScene, LoadSceneMode.Additive);
        yield return null;

        ScreenBlinder.Instance.BlinderOn();
        BKST.UISystemBK.Instance.HidePanel(GlobalValues.PANEL_LOADING, false);
        yield return null;

        yield return ScreenBlinder.Instance.BlinderFadeOut();

        isLoading = false;
    }
}
