using UnityEngine;

public class LayeredPrefab : MonoBehaviour
{
    [BitMask(typeof(SceneLayerType))]
    public SceneLayerType viewLayer; //해당 secneType에서 활성화 하겠다는 의미
    
    [BitMask(typeof(SceneLayerType))]
    public SceneLayerType tweenLayer;//해당 secneType에서 tween 기능 사용하겠다는 의미

    public GameObject prefab;//인스턴싱 할 UI prefab

    public string rename;   //이름 다시 명명
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;
    public bool removeOnSceneLoad = true;

    [HideInInspector]
    public GameObject go;

    private UITweener _tween;
    private bool _deactiveOnFinish = false;


    public void ChangeLayer(SceneLayerType newLayer, bool skipTween = false)
    {
        if(newLayer != SceneLayerType.None && (viewLayer & newLayer) == newLayer)
        {
            if(go != null && go.activeSelf)
            {
                // 이전 레이어에서 트윈이 플레이 되었지만.. 새로운 레이어에서는 플레이 되지 않는다면 되돌린다.
                if (_tween != null && _tween.tweenFactor > 0.99f && (tweenLayer & newLayer) != newLayer)
                {
                    _deactiveOnFinish = false;

                    _tween.PlayReverse();
                    _tween.ResetToBeginning();
                }
            }
            else
            {
                ///////////////////////////////////////////////////////////////////
                if ( go == null )
                {
                    go = Instantiate(prefab) as GameObject;
                    if(go != null)
                    {
                        if(!string.IsNullOrEmpty(rename))
                        {
                            go.name = rename;
                        }

                        go.transform.parent = transform;
                        go.transform.localPosition = position;
                        go.transform.localRotation = Quaternion.Euler(rotation);
                        go.transform.localScale = scale;

                        _tween = go.GetComponent<UITweener>();
                        if(_tween != null)
                        {
                            _tween.AddOnFinished(DeactiveFromTween);
                        }

                        Utility.ChangeLayerRecursively(go.transform, transform.gameObject.layer);
                    }
                }
                else
                {
                    go.SetActive(true);
                }
                ///////////////////////////////////////////////////////////////////
                if(_tween != null && (tweenLayer & newLayer) == newLayer )
                {
                    _deactiveOnFinish = false;

                    if(skipTween)
                    {
                        float keep = _tween.delay;

                        _tween.delay = 0.0f;
                        _tween.tweenFactor = 1.0f;

                        _tween.PlayForward();
                        _tween.delay = keep;
                    }
                    else
                    {
                        _tween.PlayForward();
                        _tween.ResetToBeginning();
                    }
                }
                ///////////////////////////////////////////////////////////////////
            }
        }
        else//안보여야 하는 경우
        {
            if (go != null)
            {
                if (_tween != null && _tween.tweenFactor > 0.99f)
                {
                    _deactiveOnFinish = true;

                    float keep = _tween.delay;

                    if (skipTween)
                    {
                        _tween.delay = 0.0f;
                        _tween.tweenFactor = 0.0f;
                    }
                    else
                    {
                        _tween.delay = 0.0f;
                        _tween.tweenFactor = 1.0f;
                    }

                    _tween.PlayReverse();
                    _tween.delay = keep;
                }
                else
                {
                    go.SetActive(false);
                }
            }
        }
        
    }

    public void RemoveOnSceneLoad()
    {
        if( go != null && removeOnSceneLoad)
        {
            DestroyImmediate(go);
            go = null;
        }
    }
    
    private void DeactiveFromTween()
    {
        if(_deactiveOnFinish)
        {
            Deactive();
        }
    }

    public void Deactive()
    {
        if(go != null)
        {
            go.SetActive(false);
        }
    }

    public void Clear()
    {
        if(go != null)
        {
            Destroy(go);
            go = null;
        }
    }

}
