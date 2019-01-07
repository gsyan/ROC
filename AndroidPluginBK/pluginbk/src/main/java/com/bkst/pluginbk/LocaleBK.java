package com.bkst.pluginbk;

import java.util.Locale;
import org.json.JSONObject;
import android.util.Log;
import com.unity3d.player.UnityPlayer;

public class LocaleBK {

    public static void GetLocaleList() {
        Log.d("bk", "GetLocaleList");

        Locale lo = Locale.getDefault();

        String iso = lo.getISO3Country();
        String code = lo.getCountry();
        String name = lo.getDisplayCountry();
        String lang = lo.getLanguage();

        try {
            JSONObject localeData = new JSONObject();
            localeData.put("iso", iso);
            localeData.put("code", code);
            localeData.put("name", name);
            localeData.put("lang", lang);
            UnityPlayer.UnitySendMessage("_NativeBridge", "SetLocale", localeData.toString());
            Log.d("bk", "UnitySendMessage -> _NativeBridge / SetLocale");
        }
        catch(Exception e)
        {
            Log.d("bk", "GetLocalesList() error: " + e);
        }
    }
}

