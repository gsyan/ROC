using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 포커싱 된 ui 외에는 눌리지 않도록 막는 장치
/// 
/// </summary>

public class UIBlackImageController : MonoBehaviour
{
    [SerializeField]
    private Button _blackImage = null;

    private Transform _cachedTransform; //_blockScreen 가 원래의 부모 밑으로 복귀를 위해서 필요하다.
    private Transform _blackImageTransform;

    private void Awake()
    {
        _cachedTransform = transform;
        _blackImageTransform = _blackImage.transform;
        _blackImage.gameObject.SetActive(false);
    }

    public void Setup(UIPanelBase target)
    {
        Transform parent = _cachedTransform;
        bool active = false;

        if (target != null)
        {
            active = true;
            Transform depth = target.transform.parent;
            parent = depth.GetChild(0);
        }

        Utility.ChangeLayerRecursively(_blackImageTransform, parent.gameObject.layer);
        Utility.SetParent(parent, _blackImageTransform, false);
        _blackImage.gameObject.SetActive(active);

    }

    public void SetAlpha(float alpha)
    {
        Color color = _blackImage.image.color;
        color.a = alpha;
        _blackImage.image.color = color;
    }



}
