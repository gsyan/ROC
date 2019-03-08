using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UnityEventBK : MonoBehaviour
{

    //이 스크립트를 컴포넌트화 하면 인스팩터에 OnAspectChanged 가 노출되어 사용 가능
    //사용 ((MonoBehaviour)onAspectChanged.GetPersistentTarget(0)).SendMessage(onAspectChanged.GetPersistentMethodName(0), 인자);
    [System.Serializable]
    public class OnAspectChanged : UnityEvent<float> { };
    public OnAspectChanged onAspectChanged;

    public void Execute()
    {
        ((MonoBehaviour)onAspectChanged.GetPersistentTarget(0)).SendMessage(onAspectChanged.GetPersistentMethodName(0), 1.5f);
    }

    
}
