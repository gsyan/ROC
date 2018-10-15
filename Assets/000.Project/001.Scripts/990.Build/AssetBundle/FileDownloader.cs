using System.Collections;
using UnityEngine;
using System;//Uri
using System.IO;//Stream
//using System.Net;//WebExceptionStatus
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{
    private static FileDownloader _instance;
    public static FileDownloader Instance
    {
        get
        {
            if( _instance == null)
            {
                GameObject obj = new GameObject("FileDownloader");
                _instance = obj.AddComponent<FileDownloader>();
                //DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void GetText(string url, Callback done, Callback fail, ref Byte[] res)
    {
        StartCoroutine(GetTextCo(url, done, fail, res));
    }
    IEnumerator GetTextCo(string url, Callback done, Callback fail, Byte[] res)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            fail();
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            done();
        }
    }




    IEnumerator GetAssetBundleCo()
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


