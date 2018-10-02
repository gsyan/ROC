using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIRootCI : MonoBehaviour
{
    public GameObject ci;   //회사마크 이미지
    
    IEnumerator Start()
    {
        ci.SetActive(true);
        
        yield return ScreenBlinder.Instance.BlinderFadeOut();//블라인더 있다가 사라지면서
        
        yield return new WaitForSeconds(2.0f);//CI가 선명해진 상태로 2초
        
        yield return ScreenBlinder.Instance.BlinderFadeIn();//블라인더 다시 생김

        DLog.LogMSG("UIRootCI end");

        SceneManager.LoadScene("Patch");
    }
}
