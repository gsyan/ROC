using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheFirst : MonoBehaviour
{
    private void Start()
    {
        //절전 안되게
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        GameSettings.Instance.Apply();

        NativeBridge.Instance.Init();

        SceneManager.LoadScene("CI");
    }



}
