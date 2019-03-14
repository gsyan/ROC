using UnityEngine;
using System.Collections;

public class StaticManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetMainPlugin();

    }
	
    private void SetMainPlugin()
    {
        // 안드로이드 activity나 이를 상속받은 UnityPlayerActivity 를 상속받은 클래스의
        // 메소드를 쓰기 위해서는 아래 처럼 해야 한다.
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ajObject = jc.GetStatic<AndroidJavaObject>("currentActivity");

        // 아무것도 상속받지 않은 클래스에 속한 메소드를 호출하기 위해서는
        // 아래와 같이 클래스까지의 경로(패키지명)을 지정한다.
        //ajObject = new AndroidJavaObject("com.bkst.pluginunity.MainActivity");


        //ajObject.CallStatic("LogStatic", "testStatic");
        ajObject.Call("TestFunc");
    }


	// Update is called once per frame
	void Update () {
	
	}
}
