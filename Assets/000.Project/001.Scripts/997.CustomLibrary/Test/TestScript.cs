using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;//StringBuilder
using System.Net;//HttpWebRequest
using System.IO;//Stream
using UnityEngine.Networking;

public class TestScript : MonoBehaviour
{
    string url = "https://cdn.jsdelivr.net/gh/gsyan/ROCPatch@v0.0.1.1/patch/android_test/server_condition.json";

    void Start()
    {
        //FileDownloader.Instance.GetText(url);
        //StartCoroutine(GetText());



    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    IEnumerator GetAssetBundle()
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("http://www.my-server.com/myData.unity3d");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        }
    }

}
