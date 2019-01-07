package com.bkst.pluginbk;

import android.content.Context;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayerActivity;

public class Bridge extends UnityPlayerActivity {
    //빌드는 우측 Gradle 이하 pluginbk / Tasks / build / assemble 더블클릭 -> debug, release 모두 생성됨

    private  static String unityObjName = "";
    private  static String unityFuncName = "";
    private  static Bridge gInstance = null;

    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);

        gInstance = this;

        Log.d("bk", "Bridge / onCreate ");
    }




    public void ToastString(final Context ctx, final String message)
    {
        Log.d("bk", "Bridge / ToastString ");

        new Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(ctx, message, Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void OpenNativeWebView(final String url)
    {
        Log.d("bk", "Bridge / OpenNativeWebView ");
    }

    public void LogMSG(final String msg)
    {
        Log.d("bk", msg );
    }



}
