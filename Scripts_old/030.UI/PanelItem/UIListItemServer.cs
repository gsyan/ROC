using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListItemServer : MonoBehaviour
{
    public UILabel labelName;

    private int index;

    public void Setup(int index, string name)
    {
        this.index = index;
        labelName.text = name;
    }

    public void OnChange()
    {
        //DLog.LogMSG("UIListItemServer / OnChange");

        Messenger<int>.Broadcast(EventKey.SelectServer, index);
    }


}
