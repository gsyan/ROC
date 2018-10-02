using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewStart : MonoBehaviour
{
    
    IEnumerator Start()
    {
        GInfo.Clear();

        //ScreenBlinder.Instance.BlinderOn();



        //yield return ScreenBlinder.Instance.BlinderFadeOut();


        yield return ScreenBlinder.Instance.BlinderFadeIn();
        SceneLoad.nextScene = "CampYakSan";
        SceneManager.LoadScene("loading");

        
    }

    IEnumerator LoadingScene(string sceneName)
    {
        yield return ScreenBlinder.Instance.BlinderFadeIn();

        SceneLoad.nextScene = sceneName;
        SceneManager.LoadScene("loading");

    }









}
