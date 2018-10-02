using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPatchImage : MonoBehaviour
{
    public float intervalTime = 0.0f;
    public int imageCount = 1;

    private Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
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
                //_image.sprite.texture = tex;
            }

            number++;
            if (number > imageCount)
            {
                number = 1;
            }
        }
    }


}
