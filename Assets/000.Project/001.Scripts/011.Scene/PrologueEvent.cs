using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueEvent : MonoBehaviour
{
    public static bool showPrologue = true;
    public readonly int index_0 = 0;
    public readonly int index_1 = 1;
    public readonly int index_2 = 2;

    public readonly bool activeTrue = true;
    public readonly bool activeFalse = true;
    public bool isTest = false;

    
    public GameObject[] sequences;
    public GameObject BGM;
    public GameObject eventObjects;


    private Callback _endCallBack;

    private void OnEnable()
    {
        Messenger<int, bool>.AddListener(EventKey.TutorialSequenceActive, OnSequence);
    }
    private void OnDisable()
    {
        Messenger<int, bool>.RemoveListener(EventKey.TutorialSequenceActive, OnSequence);
    }

    private void Start()
    {
        if(isTest)
        {
            //Setup(null);
        }
    }
    //public void Setup(ClientActor actor, Callback endCall = null)
    //{
    //    _player = actor;
    //    _endCallBack = endCall;
    //    if(_player == null)
    //    {
            
    //    }


    //}




    public void OnSequence(int index, bool active = true)
    {
        if(index < sequences.Length)
        {
            sequences[index].SetActive(active);
        }
    }





}
