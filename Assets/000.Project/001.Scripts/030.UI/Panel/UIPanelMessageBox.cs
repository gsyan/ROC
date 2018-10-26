using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UIPanelMessageBox : UIPanelBase
{
    public Callback onYesCallback;
    public Callback onNoCallback;
    public Callback onOkCallback;
    public Callback onCancelCallback;
    public Callback onCustom_1Callback;
    public Callback onCustom_2Callback;
    public Callback onDeactiveCallback;

    public Text title;
    public Text message;
    public Text custom_1;
    public Text custom_2;
    public GameObject buttonYes;
    public GameObject buttonNo;
    public GameObject buttonOK;
    public GameObject buttonCancel;
    public GameObject buttonCustom_1;
    public GameObject buttonCustom_2;
    
    private void OnDestroy()
    {
        if (onDeactiveCallback != null)
        {
            onDeactiveCallback();
        }
    }


    public override void OnDeactive()
    {
        base.OnDeactive();

        if (onDeactiveCallback != null)
        {
            onDeactiveCallback();
            onDeactiveCallback = null;
        }
    }

    private void Reset()
    {
        buttonOK.SetActive(false);
        buttonCancel.SetActive(false);
        buttonYes.SetActive(false);
        buttonNo.SetActive(false);
        buttonCustom_1.SetActive(false);
        buttonCustom_2.SetActive(false);
        
        onOkCallback = null;
        onCancelCallback = null;
        onYesCallback = null;
        onNoCallback = null;
        onCustom_1Callback = null;
        onCustom_2Callback = null;
        onDeactiveCallback = null;
    }



    public void Setup(string title, string message, MessageBoxType type)
    {
        Reset();

        this.title.text = title;
        this.message.text = message;

        switch(type)
        {
            case MessageBoxType.Ok:
                buttonOK.SetActive(true);
                buttonOK.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;

            case MessageBoxType.OkCancel:
                buttonOK.SetActive(true);
                buttonCancel.SetActive(true);
                buttonOK.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(250, 0);
                buttonCancel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, 0);
                break;

            case MessageBoxType.YesNo:
                buttonYes.SetActive(true);
                buttonNo.SetActive(true);
                buttonYes.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(250, 0);
                buttonNo.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, 0);
                break;

            case MessageBoxType.YesNoCancel:
                buttonYes.SetActive(true);
                buttonNo.SetActive(true);
                buttonCancel.SetActive(true);
                buttonYes.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(350, 0);
                buttonNo.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                buttonCancel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350, 0);
                break;
        }

    }

    public void SetupCustom(string title, string message, string buttonMessage_1, string buttonMessage_2)
    {
        Reset();

        this.title.text = title;
        this.message.text = message;

        buttonCustom_1.SetActive(true);
        buttonCustom_2.SetActive(true);

        buttonCustom_1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(250, 0);
        buttonCustom_2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, 0);


        this.custom_1.text = buttonMessage_1;
        this.custom_2.text = buttonMessage_2;
    }


    public void OnYes()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
        if (onYesCallback != null)
        {
            onYesCallback();
        }
    }

    public void OnNo()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
        if (onNoCallback != null)
        {
            onNoCallback();
        }
    }

    public void OnOk()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
        if (onOkCallback != null)
        {
            onOkCallback();
        }
    }

    public void OnCancel()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
        if (onCancelCallback != null)
        {
            onCancelCallback();
        }
    }

    public void OnCustom1()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
        if (onCustom_1Callback != null)
        {
            onCustom_1Callback();
        }
    }

    public void OnCustom2()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
        if (onCustom_2Callback != null)
        {
            onCustom_2Callback();
        }
    }

    public void OnX()
    {
        BKST.UISystem.Instance.HidePanel(transform, false);
    }

}


