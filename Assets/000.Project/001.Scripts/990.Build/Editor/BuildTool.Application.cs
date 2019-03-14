using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


[System.Flags]
public enum DefineSymbolType
{
    UNITY_DEBUG = 1,
    USE_ASSET_BUNDLE = 2,
    USE_AD = 4,
    USE_IAP = 8,
    USE_BETATEST = 16,
    USE_ONLYONCE = 32,
}

public enum GameServerType
{
    GAMESERVER_ALL = 0,
    GAMESERVER_QA,
    GAMESERVER_LIVE,
}

public enum BillingType
{
    BILLING_UNITY = 0,
    BILLING_NSTORE,
    BILLING_ONESTORE,
}

public enum LoginType
{
    LOGIN_FACEBOOK = 0,
    LOGIN_KAKAO,
    LOGIN_GUEST,
}


public partial class BuildTool : EditorWindow
{
    void DrawApplicationGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("■ Define Symbol( All Selectable )");

        DefineSymbolType selectedSymbols = (DefineSymbolType)PlayerPrefs.GetInt("selected_define_symbol", 0);
        foreach (DefineSymbolType symbol in Enum.GetValues(typeof(DefineSymbolType)))
        {
            bool toggle = EditorGUILayout.ToggleLeft(" " + symbol.ToString(), (selectedSymbols & symbol) == symbol);
            if (toggle)
            {
                selectedSymbols |= symbol;
            }
            else
            {
                selectedSymbols &= ~symbol;
            }
        }
        PlayerPrefs.SetInt("selected_define_symbol", (int)selectedSymbols);

        //서비스 종류( 테스트, 상용 )/////////////////////////////////////////////////////////////
        GUILayout.Space(10);
        EditorGUILayout.LabelField("■ Define Server( Only One )");

        int selectedService = PlayerPrefs.GetInt("selected_service_define_symbol", 0);

        int index = 0;
        foreach (GameServerType type in Enum.GetValues(typeof(GameServerType)))
        {
            bool toggle = index == selectedService;

            bool newValue = EditorGUILayout.ToggleLeft(" " + type.ToString(), toggle);
            if (toggle != newValue)
            {
                selectedService = index;
                PlayerPrefs.SetInt("selected_service_define_symbol", selectedService);
            }

            index++;
        }

        //스토어 종류/////////////////////////////////////////////////////////////
#if UNITY_ANDROID
        GUILayout.Space(10);
        EditorGUILayout.LabelField("■ Define Store( Only One )");

        int selectedStore = PlayerPrefs.GetInt("android_billing_store", 0);

        index = 0;
        foreach (BillingType type in Enum.GetValues(typeof(BillingType)))
        {
            bool toggle = index == selectedStore;

            bool newValue = EditorGUILayout.ToggleLeft(" " + type.ToString(), toggle);
            if (toggle != newValue)
            {
                selectedStore = index;
                PlayerPrefs.SetInt("android_billing_store", selectedStore);
            }

            index++;
        }
#else
		int selectedStore = (int)AndroidBillingType.BILLING_UNITY;
#endif

        //소셜 종류/////////////////////////////////////////////////////////////
#if UNITY_ANDROID
        GUILayout.Space(10);
        EditorGUILayout.LabelField("■ Define Social( Only One )");

        int selectedSocial = PlayerPrefs.GetInt("android_social", 0);

        index = 0;
        foreach (LoginType type in Enum.GetValues(typeof(LoginType)))
        {
            bool toggle = index == selectedSocial;

            bool newValue = EditorGUILayout.ToggleLeft(" " + type.ToString(), toggle);
            if (toggle != newValue)
            {
                selectedSocial = index;
                PlayerPrefs.SetInt("android_social", selectedSocial);
            }

            index++;
        }
#elif UNITY_IPHONE
        int selectedSocial = (int)DefineSocialType.SOCIAL_GUEST_ONLY;
#else
		int selectedSocial = (int)DefineSocialType.SOCIAL_GUEST_ONLY;
#endif


        GUILayout.Space(5);
        if (GUILayout.Button("Build Application"))
        {
            // remake symbols
            string oldDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            List<string> defineSymbolList = new List<string>(oldDefineSymbols.Split(';'));

            // make define symbol
            foreach (DefineSymbolType symbol in Enum.GetValues(typeof(DefineSymbolType)))
            {
                if ((selectedSymbols & symbol) == symbol)
                {
                    if (!defineSymbolList.Contains(symbol.ToString()))
                    {
                        defineSymbolList.Add(symbol.ToString());
                    }
                }
                else
                {
                    if (defineSymbolList.Contains(symbol.ToString()))
                    {
                        defineSymbolList.Remove(symbol.ToString());
                    }
                }
            }

            foreach (GameServerType type in Enum.GetValues(typeof(GameServerType)))
            {
                if (selectedService == (int)type)
                {
                    if (!defineSymbolList.Contains(type.ToString()))
                    {
                        defineSymbolList.Add(type.ToString());
                    }
                }
                else
                {
                    if (defineSymbolList.Contains(type.ToString()))
                    {
                        defineSymbolList.Remove(type.ToString());
                    }
                }
            }

            foreach (BillingType type in Enum.GetValues(typeof(BillingType)))
            {
                if (selectedStore == (int)type)
                {
                    if (!defineSymbolList.Contains(type.ToString()))
                    {
                        defineSymbolList.Add(type.ToString());
                    }
                }
                else
                {
                    if (defineSymbolList.Contains(type.ToString()))
                    {
                        defineSymbolList.Remove(type.ToString());
                    }
                }
            }


            foreach (LoginType type in Enum.GetValues(typeof(LoginType)))
            {
                if (selectedSocial == (int)type)
                {
                    if (!defineSymbolList.Contains(type.ToString()))
                    {
                        defineSymbolList.Add(type.ToString());
                    }
                }
                else
                {
                    if (defineSymbolList.Contains(type.ToString()))
                    {
                        defineSymbolList.Remove(type.ToString());
                    }
                }
            }




            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < defineSymbolList.Count; i++)
            {
                if (defineSymbolList[i] != "")
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(";" + defineSymbolList[i]);
                    }
                    else
                    {
                        sb.Append(defineSymbolList[i]);
                    }
                }
            }

            string newDefineSymbols = sb.ToString();
            Debug.Log("Build DefineSymbols: " + newDefineSymbols);

            // Build
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, newDefineSymbols);
            if ((selectedSymbols & DefineSymbolType.USE_ASSET_BUNDLE) == DefineSymbolType.USE_ASSET_BUNDLE)
            {
                BuildWithoutResource();
            }
            else
            {
                BuildWithResource();
            }

            // roll back define symbols
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, oldDefineSymbols);
            AssetDatabase.SaveAssets();//bk, EditorApplication.SaveAssets(); 대신
        }
    }


    void BuildWithResource()
    {
        try
        {
            string apkBuildPath = PlayerPrefs.GetString("apk_build_path", _rootDir);
            apkBuildPath = EditorUtility.SaveFilePanel("Build", apkBuildPath, "", "apk");
            Debug.Log(apkBuildPath);

            if (string.IsNullOrEmpty(apkBuildPath))
            {
                return;
            }

            PlayerPrefs.SetString("apk_build_path", apkBuildPath);

            // Media 폴더 이동
            AssetDatabase.MoveAsset(RESOURCE_MEDIA_DIR, STREAMING_MEDIA_DIR);
            AssetDatabase.Refresh();

#if UNITY_ANDROID
            PushAndroidSigning();
#endif
            // 빌드
            string[] levels = GetLevels();
            BuildPipeline.BuildPlayer(levels, apkBuildPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

#if UNITY_ANDROID
            PopAndroidSigning();
#endif
        }
        catch (Exception e)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("빌드 실패", e.Message, "확인");
        }
        finally
        {
            // Media 폴더 이동
            AssetDatabase.MoveAsset(STREAMING_MEDIA_DIR, RESOURCE_MEDIA_DIR);
            AssetDatabase.Refresh();
        }
    }


    void BuildWithoutResource()
    {
        try
        {
            string apkBuildPath = PlayerPrefs.GetString("apk_build_path", _rootDir);
            apkBuildPath = EditorUtility.SaveFilePanel("Build", apkBuildPath, "", "apk");

            if (string.IsNullOrEmpty(apkBuildPath))
            {
                return;
            }

            PlayerPrefs.SetString("apk_build_path", apkBuildPath);


            // change streaming to temp
            EditorUtility.DisplayProgressBar("빌드", "스트리밍 폴더 이동중...", 0.1f);
            string error = AssetDatabase.MoveAsset(STREAMING_DIR, STREAMING_TEMP_DIR);
            if (error != "")
            {
                throw new Exception(error);
            }


            // change reousrce to temp
            EditorUtility.DisplayProgressBar("빌드", "리소스 폴더 이동중...", 0.1f);
            error = AssetDatabase.MoveAsset(RESOURCE_DIR, RESOURCE_TEMP_DIR);
            if (error != "")
            {
                throw new Exception(error);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

#if UNITY_ANDROID
            PushAndroidSigning();
#endif

            // 빌드
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetLevels();
            buildPlayerOptions.locationPathName = apkBuildPath;
            buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildPlayerOptions.options = BuildOptions.None;
            BuildPipeline.BuildPlayer(buildPlayerOptions);


            if (error != "")
            {
                throw new Exception(error);
            }

#if UNITY_ANDROID
            PopAndroidSigning();
#endif

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("빌드", "완료", "확인");
            EditorUtility.RevealInFinder(apkBuildPath);
        }
        catch (Exception e)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("빌드 실패", e.Message, "확인");
        }
        finally
        {
            //EditorUtility.DisplayProgressBar("빌드", "임시 폴더 복구중...", 0.3f);

            if (Directory.Exists(STREAMING_TEMP_DIR))
            {
                AssetDatabase.MoveAsset(STREAMING_TEMP_DIR, STREAMING_DIR);
            }

            if (Directory.Exists(RESOURCE_TEMP_DIR))
            {
                AssetDatabase.MoveAsset(RESOURCE_TEMP_DIR, RESOURCE_DIR);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }


    string[] GetLevels()
    {
        List<string> levels = new List<string>();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                levels.Add(EditorBuildSettings.scenes[i].path);
            }
        }

        return levels.ToArray();
    }


    bool ReadValue(string key, bool defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }


    void WriteValue(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }


    void PushAndroidSigning()
    {
        string keystorePath = Directory.GetCurrentDirectory() + "/Certificate/roc.keystore";

        PlayerSettings.Android.keystoreName = keystorePath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        PlayerSettings.Android.keyaliasName = "roc";
        PlayerSettings.Android.keyaliasPass = "bkst81";
        PlayerSettings.Android.keystorePass = "bkst81";

        Debug.Log(PlayerSettings.Android.keystoreName);
    }


    void PopAndroidSigning()
    {
        PlayerSettings.Android.keystoreName = "";
        PlayerSettings.Android.keyaliasName = "";
        PlayerSettings.Android.keyaliasPass = "";
        PlayerSettings.Android.keystorePass = "";
    }
}
