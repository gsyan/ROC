using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPatchMessageBox : MonoBehaviour
{
    public enum MessageType
    {
        Ok,
        YesNo,
    }

    [SerializeField]
    private UILabel labelMessage = null;
    [SerializeField]
    private GameObject buttonOK = null;
    [SerializeField]
    private GameObject buttonYes = null;
    [SerializeField]
    private GameObject buttonNo = null;

    public Callback onOK;
    public Callback onYes;
    public Callback onNo;
    public Callback onCancel;
    
    public void Show(string message, MessageType type)
    {
        labelMessage.text = message;

        switch(type)
        {
            case MessageType.Ok:
                buttonOK.SetActive(true);
                buttonYes.SetActive(false);
                buttonNo.SetActive(false);
                break;

            case MessageType.YesNo:
                buttonOK.SetActive(false);
                buttonYes.SetActive(true);
                buttonNo.SetActive(true);
                break;
        }
    }

    public void OnOK()
    {
        gameObject.SetActive(false);

        if (onOK != null)
        {
            onOK();
        }
    }

    public void OnYes()
    {
        gameObject.SetActive(false);

        if (onYes != null)
        {
            onYes();
        }
    }


    public void OnNo()
    {
        gameObject.SetActive(false);

        if (onNo != null)
        {
            onNo();
        }
    }


    public void OnClose()
    {
        gameObject.SetActive(false);

        if (onCancel != null)
        {
            onCancel();
        }
    }




}
