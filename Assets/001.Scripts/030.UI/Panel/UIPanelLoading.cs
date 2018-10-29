using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelLoading : UIPanelBase
{
    public Image image;
    public Text tooltip;

    public int imageCount = 1;
    public int textCount = 1;

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnActive()
    {
        base.OnActive();

        try
        {
            string str = "LoadingImage/tooltip_" + Random.Range(1, imageCount).ToString();
            Sprite sprite = ResourceSystem.Load<Sprite>(str, true);

            if (sprite != null)
            {
                Resources.UnloadAsset(image.sprite);
                image.sprite = sprite;
            }

            str = "tooltip_" + Random.Range(1, textCount).ToString();
            tooltip.text = Localization.Get(str);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    
    private void OnEnable() {}
    
}
