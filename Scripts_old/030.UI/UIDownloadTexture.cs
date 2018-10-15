using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script that shows how to download a remote texture and assign it to be used by a UITexture.
/// </summary>

[RequireComponent(typeof(UITexture))]
public class UIDownloadTexture : MonoBehaviour
{
    public bool pixelPerfect = false;

    private UITexture _ut;
    private UITexture ut
    {
        get
        {
            if (_ut == null)
            {
                _ut = GetComponent<UITexture>();
            }
            return _ut;
        }
        //set
        //{
        //	_ut = value;
        //}
    }

    private Callback<Texture2D> onCallbackFinish = null;
    private Coroutine coroutine;


    void OnEnable()
    {
        if (ut != null)
        {
            ut.enabled = true;
        }
    }


    void OnDisable()
    {
        if (ut != null)
        {
            ut.enabled = false;
        }
    }


    public void UpdateTexture(Texture2D tex)
    {
        ut.mainTexture = tex;
        if (pixelPerfect)
        {
            ut.MakePixelPerfect();
        }
    }


    public void UpdateTexture(string url, Callback<Texture2D> a_onFinish = null)
    {
        ut.mainTexture = null;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        onCallbackFinish = a_onFinish;

        if (!string.IsNullOrEmpty(url) && gameObject.activeInHierarchy)
        {
            enabled = true;
            coroutine = StartCoroutine(DownloadImage(url));
        }
        else
        {
            if (onCallbackFinish != null)
            {
                onCallbackFinish(null);
            }
        }
    }


    IEnumerator DownloadImage(string url)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;

            Texture2D tex2D = null;

            if (string.IsNullOrEmpty(www.error))
            {
                tex2D = www.texture;

                if (tex2D != null)
                {
                    UpdateTexture(tex2D);
                }
            }

            if (onCallbackFinish != null)
            {
                onCallbackFinish(tex2D);
            }
        }
    }
}
