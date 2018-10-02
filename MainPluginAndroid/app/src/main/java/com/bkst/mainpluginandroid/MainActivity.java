package com.bkst.mainpluginandroid;



import android.os.AsyncTask;//구글 광고 아이디 받을때 씀
import android.os.Bundle;
import android.util.Log;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import com.google.android.gms.common.GooglePlayServicesNotAvailableException;
import com.google.android.gms.common.GooglePlayServicesRepairableException;
//import com.google.android.gms.gcm.GoogleCloudMessaging;

import java.util.Locale;
import org.json.JSONObject;


//주석 설정 : Control + /
public class MainActivity extends UnityPlayerActivity
{
    private  static String unityObjectName = "";
    private  static String unityCallbackName = "";
    private  static MainActivity gInstance = null;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        gInstance = this;

        Log.d("bk", "MainActivity / onCreate");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
    }

    @Override
    protected void onPause() {
        super.onPause();
    }

    @Override
    protected void onResume() {
        super.onResume();
    }



    public  static void LogStatic(String str)
    {
        Log.d("bk", str);
    }
    public void Log(String str)
    {
        Log.d("bk", str);
    }

    public static void GetGAID(String a_unityObject, String a_unityFuncName){
        unityObjectName = a_unityObject;
        unityCallbackName = a_unityFuncName;

        gInstance.ProcessGetGAID();
    }
    public  void ProcessGetGAID(){
        AsyncTask<Void, Void, String> task = new AsyncTask<Void, Void, String>() {
            @Override
            protected String doInBackground(Void... voids) {

                AdvertisingIdClient.Info idInfo = null;
                try {
                    idInfo = AdvertisingIdClient.getAdvertisingIdInfo(getApplicationContext());
                }catch (GooglePlayServicesNotAvailableException e) {
                    Log.d("bk", "error: " + e.getMessage());
                } catch (GooglePlayServicesRepairableException e) {
                    Log.d("bk", "error: " + e.getMessage());
                } catch (Exception e) {
                    Log.d("bk", "error: " + e.getMessage());
                }
                String advertId = null;
                try{
                    advertId = idInfo.getId();
                }catch (Exception e){
                    Log.d("bk", "error: " + e.getMessage());
                }

                UnityPlayer.UnitySendMessage(unityObjectName, unityCallbackName, advertId);

                return advertId;
            }

            @Override
            protected void onPostExecute(String advertId) {
                super.onPostExecute(advertId);
            }
        };
        task.execute();
    }

    public void GetLocalesList(){

        Locale currentLocale = Locale.getDefault();
        String iso = currentLocale.getISO3Country();
        //Log.d("bk", "iso: " + iso);
        String code = currentLocale.getCountry();
        //Log.d("bk", "code: " + code);
        String name = currentLocale.getDisplayCountry();
        //Log.d("bk", "name: " + name);
        String lang = currentLocale.getLanguage();
        //Log.d("bk", "lang: " + lang);

//		String lcaleString = "{\"iso\":\""+iso+"\","
//				     	  	 +"\"code\":\""+code+"\","
//				     	  	 +"\"name\":\""+name+"\","
//				     	  	 +"\"lang\":\""+lang+"\"}";
//		Log.d("bk", "lcaleString: " + lcaleString);
//		UnityPlayer.UnitySendMessage("BKNativeUtil", "SetLocale", lcaleString);

        try
        {
            JSONObject localeData = new JSONObject();
            localeData.put("iso", iso);
            localeData.put("code", code);
            localeData.put("name", name);
            localeData.put("lang", lang);

            //Log.d("bk", "lcaleString: " + localeData.toString());
            UnityPlayer.UnitySendMessage("BKNativeUtil", "SetLocale", localeData.toString());
        }
        catch(Exception e)
        {
            Log.d("bk", "GetLocalesList() error: " + e);
        }
    }









}
