using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceProcessor
{
    private List<Callback> _sequenceList = new List<Callback>();
    private Callback<bool> _finishCallback;
    private int _index = 0;

    public int Count
    {
        get
        {
            return _sequenceList.Count;
        }
    }
    public void Add(Callback process)
    {
        _sequenceList.Add(process);
    }
    public void Clear()
    {
        _sequenceList.Clear();
    }

    public void Start(Callback<bool> callback)
    {
        _finishCallback = callback;
        _index = 0;
        NextProcess();
    }
    /// <summary>
    /// 등록된 각각의 프로세스들 내부적으로 성공시 SequenceProcessor.NextProcess를 호출하도록 해야함
    /// </summary>
    public void NextProcess()
    {
        if(_index < _sequenceList.Count)
        {
            _sequenceList[_index++]();
        }
        else
        {
            if(_finishCallback != null)
            {
                _finishCallback(true);
            }
        }
    }
    /// <summary>
    /// 등록된 각각의 프로세스들 내부적으로 실패시 SequenceProcessor.FailProcess를 호출하도록 해야함
    /// </summary>
    public void FailProcess()
    {
        if (_finishCallback != null)
        {
            _finishCallback(false);
        }
    }

	
}
