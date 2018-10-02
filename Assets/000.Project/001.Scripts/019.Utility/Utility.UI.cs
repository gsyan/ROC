using UnityEngine;
using System.Collections;


public partial class Utility
{

    public static void ShowConnecting()
    {
        BKST.UISystemBK.Instance.ShowPanel(GlobalValues.PANEL_CONNECTING);
    }
    public static void HideConnecting()
    {
        BKST.UISystemBK.Instance.HidePanel(GlobalValues.PANEL_CONNECTING, false);
    }





    public static void ShowMessageBox(string title, string message, MessageBoxType type,
                                        Callback onCallbackOk = null,
                                        Callback onCallbackCancel = null,
                                        Callback onCallbackDeactive = null)
    {
        Transform tm = BKST.UISystemBK.Instance.ShowMessageBox();
        if( tm != null)
        {
            UIPanelMessageBox mb = tm.GetComponent<UIPanelMessageBox>();
            if(mb != null)
            {
                mb.Setup(title, message, type);

                if(onCallbackOk != null)
                {
                    mb.onOkCallback = onCallbackOk;
                    mb.onYesCallback = onCallbackOk;
                }

                if(onCallbackCancel != null)
                {
                    mb.onNoCallback = onCallbackCancel;
                    mb.onCancelCallback = onCallbackCancel;
                }

                if(onCallbackDeactive != null)
                {
                    mb.onDeactiveCallback = onCallbackDeactive;
                }
            }
            else
            {
                DLog.LogMSG("UIPanelMessageBox script not found in Panel MessageBox");
            }

        }
    }



    public static void ShowErrorCodeResult(ErrorResult a_result)
    {
        bool isDevelopment = false;
#if UNITY_EDITOR
        isDevelopment = true;
#endif

        ErrorCodeData errorCodeData = GData.Instance.GetErrorCodeData(a_result);
        if(errorCodeData == null)//클라 csv에 세팅된 에러메세지 중 없다면
        {
            if( isDevelopment )
            {
                ShowMessageBox("ErrorCodeResult", string.Format("There's No Result Type : {0}", a_result), MessageBoxType.Ok);
            }

            //없다고 해도 릴리즈 모드에서는 경고 팝업을 띄우지 않는다
            return;
        }

        if (isDevelopment == false && errorCodeData.bPopupRelease == true)//유니티는 아니고, 릴리즈 모드에서도 경고 팝업 띄우는 설정 이라면
        {
            ShowMessageBox("ErrorCodeResult", errorCodeData.message, MessageBoxType.Ok);
            return;
        }

        if (isDevelopment == true && errorCodeData.bPopupDevelopment == true)//유니티 이고, 디버그 모드에서 경고 팝업 띄우는 설정 이라면
        {
            ShowMessageBox("ErrorCodeResult", errorCodeData.message, MessageBoxType.Ok);
            return;
        }
    }




}


//공백문자 제거, 안녕 이것 -> 안녕이것
//str.Trim();

//대소문자
//str.ToUpper();
//str.ToLower();

//문자열 추출
//str.IndexOf();
//str.LastIndexOf();
//str.Substring(4,8);, str.Substring(4);
