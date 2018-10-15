using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPatchImage : MonoBehaviour
{
    public float intervalTime = 0.0f;
    public int imageCount = 1;

    private UITexture _image;

    void Awake()
    {
        _image = GetComponent<UITexture>();
    }

    public void StartLoopImage()
    {
        //StartCoroutine(LoopImageChange());
    }

    IEnumerator LoopImageChange()
    {
        int number = 1;

        while (true)
        {
            yield return new WaitForSeconds(intervalTime);

            Texture tex = Resources.Load("Texture/downloading_" + number) as Texture;
            if (tex != null)
            {
                Resources.UnloadAsset(_image.mainTexture);

                _image.mainTexture = tex;
            }

            number++;
            if (number > imageCount)
            {
                number = 1;
            }
        }
    }


}
