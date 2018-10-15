using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootPatch : MonoBehaviour
{
    //private 에 SerializeField 을 붙이면 인스팩터 창에서 수정이 가능함
    [SerializeField]
    private UILabel _labelState = null;
    [SerializeField]
    private UILabel _labelCount = null;
    [SerializeField]
    private UILabel _labelPercent = null;
    [SerializeField]
    private UISprite _spriteProgress = null;
    [SerializeField]
    private UIPatchMessageBox _messageBox = null;
    [SerializeField]
    private UIPatchImage _patchImage = null;

    private void Awake()
    {
        if( _messageBox != null )
        {
            _messageBox.gameObject.SetActive(false);
        }
    }

    public void SetStateText(string text)
    {
        _labelState.text = text;
    }

    public void UpdateDownloadProgress(int readByte, int totalByte)
    {
        float percent = (float)readByte / (float)totalByte;

        _spriteProgress.fillAmount = percent;
        _labelPercent.text = string.Format("{0:0.0} %", percent * 100.0f);
        _labelCount.text = string.Format("{0:0.0} / {1:0.0} MB", readByte / 1048576.0f, totalByte / 1048576.0f);
    }
    
    public void UpdateExtractProgress(int current, int total)
    {
        float percent = (float)current / (float)total;

        _spriteProgress.fillAmount = percent;
        _labelPercent.text = string.Format("{0:0.0} %", percent * 100.0f);
        _labelCount.text = string.Format("{0} / {1}", current, total);
    }

    public void ClearProgress()
    {
        _spriteProgress.fillAmount = 1.0f;
        _labelPercent.text = string.Format("{0:0.0} %", 100.0f);
        _labelCount.text = "";
    }

    public void ShowMessageBoxOK(string message, Callback onOK = null, Callback onCancel = null)
    {
        if (_messageBox != null)
        {
            _messageBox.gameObject.SetActive(true);
            _messageBox.onOK = onOK;
            _messageBox.onCancel = onCancel;
            _messageBox.Show(message, UIPatchMessageBox.MessageType.Ok);
        }
    }

    public void ShowMessageBoxYesNo(string message, Callback onYes = null, Callback onNo = null, Callback onCancel = null)
    {
        if (_messageBox != null)
        {
            _messageBox.gameObject.SetActive(true);
            _messageBox.onYes = onYes;
            _messageBox.onNo = onNo;
            _messageBox.onCancel = onCancel;
            _messageBox.Show(message, UIPatchMessageBox.MessageType.YesNo);
        }
    }

    public void StartLoopImage()
    {
        if(_patchImage != null)
        {
            _patchImage.StartLoopImage();
        }
    }

}
