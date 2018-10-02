using UnityEngine;
using System;                       // for WeakReference
using System.Collections;
using System.Collections.Generic;   // for KeyValuePair
using System.IO;                    // for MemoryStream
using System.Text;                  // for Encoding
using ICSharpCode.SharpZipLib.Zip;  // for ZipFile

public static class ResourceSystem
{
#if UNITY_EDITOR
    public static bool useAssetBundle = false;  //patch secene start 에서 unity editor의 경우 true로 바뀐다.
#endif

    //weak reference 리스트
    private static List<KeyValuePair<string, WeakReference>> weakList = new List<KeyValuePair<string, WeakReference>>();
    public static void RegisterReference(string path, UnityEngine.Object target)
    {
        weakList.Add(new KeyValuePair<string, WeakReference>(path, new WeakReference(target)));
    }
    public static void UnloadUnusedReference()//원래 이름은 UnloadUnusedAssets, 수정완료 후 주석 삭제
    {
        KeyValuePair<string, WeakReference> pair = new KeyValuePair<string, WeakReference>("null", null);
        for( int i = weakList.Count - 1; i >= 0; --i )
        {
            pair = weakList[i];

            if( !pair.Value.IsAlive || pair.Value.Target == null || pair.Value.Target.ToString() == "null" )
            {
                weakList.RemoveAt(i);
            }
        }
    }
    public static WeakReference GetReference(string key)
    {
        KeyValuePair<string, WeakReference> pair = new KeyValuePair<string, WeakReference>("null", null);
        for (int i = weakList.Count - 1; i >= 0; --i)
        {
            pair = weakList[i];
            if(  pair.Value.IsAlive && pair.Value.Target != null && pair.Value.Target.ToString() != "null" )
            {
                if(pair.Key == key)
                {
                    return pair.Value;
                }
            }
        }

        return null;
    }


    private static ZipFile csvPack;
    private static ZipFile luaPack;

    private static string key = "+A899GRS6uVTBkLUIO0Jtbfw318p0P18ZrvINAyXSaE=";
    private static string iv = "4JXSJAJD71whV9G/eeRcJA==";



    //object 
    public static UnityEngine.Object Load(string path, bool immediateUnload = false)
    {
#if UNITY_EDITOR
        if (useAssetBundle)
        {
            UnityEngine.Object obj = AssetBundleManager.Load(path);
            if(obj != null)
            {
                if(immediateUnload)
                {
                    AssetBundleManager.UnloadAssetBundle(path);
                }
                return obj;
            }
        }
        return Resources.Load(path);
#else
    #if USE_ASSET_BUNDLE
        UnityEngine.Object obj = AssetBundleManager.Load(path);
        if (obj != null)
        {
            if (immediateUnload)
            {
                AssetBundleManager.UnloadAssetBundle(path);
            }
            return obj;
        }
        return Resources.Load(path);
    #else
        return Resources.Load(path);
    #endif
#endif 
    }
    public static T Load<T>(string path, bool immediateUnload = false) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (useAssetBundle)
        {
            T obj = AssetBundleManager.Load<T>(path);
            if (obj != null)
            {
                if (immediateUnload)
                {
                    AssetBundleManager.UnloadAssetBundle(path);
                }

                return obj;
            }
        }

        return Resources.Load<T>(path);
#else
#if USE_ASSET_BUNDLE
		T obj = AssetBundleManager.Load<T>(path);
		if (obj != null)
		{
			if (immediateUnload)
			{
				AssetBundleManager.UnloadAssetBundle(path);
			}

			return obj;
		}
		else
		{
			return Resources.Load<T>(path);
		}
#else
		return Resources.Load<T>(path);
#endif
#endif
    }
    public static UnityEngine.Object LoadRef(string path, UnityEngine.Object target)
    {
#if UNITY_EDITOR
        if (useAssetBundle)
        {
            UnityEngine.Object obj = AssetBundleManager.Load(path);
            if (obj != null)
            {
                RegisterReference(path, target);

                return obj;
            }
        }

        return Resources.Load(path);
#else
#if USE_ASSET_BUNDLE
		UnityEngine.Object obj = AssetBundleManager.Load(path);
		if (obj != null)
		{
			RegisterReference(path, target);

			return obj;
		}
		else
		{
			return Resources.Load(path);
		}
#else
		return Resources.Load(path);
#endif
#endif
    }
    public static T LoadRef<T>(string path, UnityEngine.Object target) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (useAssetBundle)
        {
            T obj = AssetBundleManager.Load<T>(path);
            if (obj != null)
            {
                RegisterReference(path, target);

                return obj;
            }
        }

        return Resources.Load<T>(path);
#else
#if USE_ASSET_BUNDLE
		T obj = AssetBundleManager.Load<T>(path);
		if (obj != null)
		{
			RegisterReference(path, target);

			return obj;
		}
		else
		{
			return Resources.Load<T>(path);
		}
#else
		return Resources.Load<T>(path);
#endif
#endif
    }

    //csv loading 시 쓰임
    public static byte[] LoadCSV(string path)
    {
#if UNITY_EDITOR
        if (useAssetBundle)
        {
            return LoadCSVFromAssetBundle(path);
        }
        else
        {
            return LoadCSVFromFile(path);
        }
#else
#if USE_ASSET_BUNDLE
		return LoadCSVFromAssetBundle(path);
#else
		return LoadCSVFromFile(path);
#endif
#endif
    }
    static byte[] LoadCSVFromAssetBundle(string path)
    {
        if (csvPack == null)
        {
            TextAsset asset = AssetBundleManager.Load<TextAsset>("csv");
            AssetBundleManager.UnloadAssetBundle("csv");

            csvPack = new ZipFile(new MemoryStream(Cryptography.DecryptBytes(asset.bytes, key, iv)));
        }

        return ReadPack(csvPack, path + ".csv");
    }
    private static byte[] LoadCSVFromFile(string path)
    {
        TextAsset asset = Resources.Load<TextAsset>("CSV/" + path);
        if(asset != null)
        {
            return asset.bytes;
        }
        
        return null;
    }



    //lua loading 시 쓰임  /////////////////////////////////////////////////////////////////////
    public static string GetLua(string path)
    {
#if UNITY_EDITOR
        if( useAssetBundle )
        {
            return LoadLuaFromAssetBundle(path);
        }
        else
        {
            return LoadLuaFromFile(path);
        }
#else
    #if USE_ASSET_BUNDLE
        return LoadLuaFromAssetBundle(path);
    #else
        return LoadLuaFromFile(path);
    #endif
#endif
    }
    private static string LoadLuaFromAssetBundle(string path)
    {
        if (luaPack == null)
        {
            TextAsset asset = AssetBundleManager.Load<TextAsset>("lua");
            AssetBundleManager.UnloadAssetBundle("lua");

            luaPack = new ZipFile(new MemoryStream(Cryptography.DecryptBytes(asset.bytes, key, iv)));
        }

        byte[] bytes = ReadPack(luaPack, path + ".lua");
        if (bytes != null)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        return "";
    }
    private static string LoadLuaFromFile(string path)
    {
        string filePath = string.Format("{0}/LuaScripts/{1}.lua", Application.streamingAssetsPath, path);
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            while (!www.isDone) { }

            return www.text;
        }
        else
        {
            return File.ReadAllText(filePath);
        }
    }
    
    private static byte[] ReadPack(ZipFile pack, string path)
    {
        ZipEntry entrty = pack.GetEntry(path);
        if (entrty != null)
        {
            using (Stream stream = pack.GetInputStream(entrty))
            {
                byte[] bytes = new byte[entrty.Size];

                int readBytes = 0;
                int totalReadBytes = 0;
                while ((readBytes = stream.Read(bytes, totalReadBytes, bytes.Length)) > 0)
                {
                    totalReadBytes += readBytes;
                }

                if (totalReadBytes == entrty.Size)
                {
                    return bytes;
                }
            }
        }

        return null;
    }





    //경로를 가지고 언로드
    public static void Unload(string path)
    {
#if UNITY_EDITOR
        if (useAssetBundle)
        {
            AssetBundleManager.UnloadAssetBundle(path, true);
        }
#else
#if USE_ASSET_BUNDLE
        AssetBundleManager.UnloadAssetBundle(path);
#endif
#endif


    }







}
