using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayTweenOnEnable : MonoBehaviour
{
    public GameObject target;
    public bool autoDeactive = false;
    public bool reverse = false;
    public bool includeChildren = true;
    public float reverseDelay = 0.0f;
    public int tweenGroup = -1;         // 음수, 원래로직 양수 트윈그룹이 같은것만

    private List<UITweener> tweenList = new List<UITweener>();
    public List<EventDelegate> onDeactiveFinish;


    void Awake()
    {
        if (target == null)
        {
            target = gameObject;
        }
    }


    void OnEnable()
    {
        tweenList.Clear();

        if (includeChildren)
        {
            UITweener[] temp = target.GetComponentsInChildren<UITweener>();
            if (tweenGroup > 0)
            {
                for (int i = 0; i < temp.Length; ++i)
                {
                    if (temp[i].tweenGroup == tweenGroup)
                    {
                        tweenList.Add(temp[i]);
                    }
                }
            }
            else
            {
                tweenList.InsertRange(0, temp);
            }
        }
        else
        {
            if (tweenGroup > 0)
            {
                UITweener[] temp = target.GetComponents<UITweener>();
                for (int i = 0; i < temp.Length; ++i)
                {
                    if (temp[i].tweenGroup == tweenGroup)
                    {
                        tweenList.Add(temp[i]);
                        break;
                    }
                }
            }
            else
            {
                tweenList.Add(target.GetComponent<UITweener>());
            }
        }

        if (tweenList.Count > 0)
        {
            for (int i = 0; i < tweenList.Count; i++)
            {
                tweenList[i].PlayForward();
                tweenList[i].ResetToBeginning();
            }

            if (autoDeactive)
            {
                StartCoroutine(AutoDeactive());
            }
        }
        else
        {
            enabled = false;
        }
    }


    IEnumerator AutoDeactive()
    {
        bool isLooping = true;

        while (isLooping)
        {
            isLooping = false;

            for (int i = 0; i < tweenList.Count; i++)
            {
                if (tweenList[i].tweenFactor < 1.0f)
                {
                    isLooping = true;
                    break;
                }
            }

            yield return null;
        }

        if (reverse)
        {
            yield return new WaitForSeconds(reverseDelay);

            for (int i = 0; i < tweenList.Count; i++)
            {
                tweenList[i].tweenFactor = 1.0f;
                tweenList[i].PlayReverse();
            }

            isLooping = true;

            while (isLooping)
            {
                isLooping = false;

                for (int i = 0; i < tweenList.Count; i++)
                {
                    if (tweenList[i].tweenFactor > 0.0f)
                    {
                        isLooping = true;
                        break;
                    }
                }

                yield return null;
            }

        }

        gameObject.SetActive(false);
        EventDelegate.Execute(onDeactiveFinish);
    }

}
