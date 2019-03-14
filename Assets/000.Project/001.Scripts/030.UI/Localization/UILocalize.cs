using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[AddComponentMenu("ROC/UI/Localize")]
public class UILocalize : MonoBehaviour
{
    public string key;

    public string value
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                Text text = GetComponent<Text>();//UILabel lbl
                Image image = GetComponent<Image>();//UISprite sp

                if (text != null)
                {
                    text.text = value;
#if UNITY_EDITOR
                    if (!Application.isPlaying) SetDirty(text);
#endif
                }
                else if (image != null)
                {
                    //image.sprite = ResourceSystem.Load<Sprite>("");
#if UNITY_EDITOR
                    if (!Application.isPlaying) SetDirty(image);
#endif
                }
            }
        }
    }

    private void SetDirty(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        if (obj)
        {
            //유니티에셋을 실행중에 값을 변경하면 값이 저장이 되지 않고 날아가는데 그 값을 디스크에 저장해서 Asset값을 바꾸는 것
            UnityEditor.EditorUtility.SetDirty(obj);
        }
#endif
    }




    private bool _started = false;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        if (_started) OnLocalize();
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        _started = true;
        OnLocalize();
    }

    private void OnLocalize()
    {
        // If no localization key has been specified, use the label's text as the key
        if (string.IsNullOrEmpty(key))
        {
            Text t = GetComponent<Text>();
            if (t != null) key = t.text;
        }

        // If we still don't have a key, leave the value as blank
        if (!string.IsNullOrEmpty(key)) value = Localization.Get(key);
    }

}
