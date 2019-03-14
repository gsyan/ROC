using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LoadedAssetBundle
{
    public AssetBundle assetBundle;
    public int referencedCount;

    public LoadedAssetBundle(AssetBundle assetBundle)
    {
        this.assetBundle = assetBundle;
        referencedCount = 1;
    }
}

public static class AssetBundleManager
{
    private static Dictionary<string, LoadedAssetBundle> _loadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();

    private static AssetBundleManifest _assetBundleManifest = null;
    public static AssetBundleManifest assetBundleManifest
    {
        set { _assetBundleManifest = value; }
    }

    static public bool Initialize()
    {
        return Initialize("assetbundle");
    }

    public static bool Initialize(string manifestAssetBundleName)
    {
        LoadedAssetBundle lab = LoadAssetBundle(manifestAssetBundleName, true);
        if(lab != null)
        {
            _assetBundleManifest = lab.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            return true;
        }
        return false;
    }

    public static void Release()
    {
        foreach (LoadedAssetBundle lab in _loadedAssetBundles.Values)
        {
            lab.assetBundle.Unload(true);
        }

        _loadedAssetBundles.Clear();
        _assetBundleManifest = null;
    }

    public static string GetAssetBundleName(string path)
    {
        string result = "assets_resources_" + path.ToLower().Replace(Path.AltDirectorySeparatorChar, '_');
        return "assets_resources_" + path.ToLower().Replace(Path.AltDirectorySeparatorChar, '_');
    }

    public static string GetMediaFilePath(string path)
    {
        return Application.persistentDataPath + "/assetbundle/assets_resources_media_" + AssetBundleUtility.GetPathWithoutExtension(path).ToLower().Replace(Path.AltDirectorySeparatorChar, '_');
    }

    public static UnityEngine.Object Load(string path)
    {
        string assetbundlename = GetAssetBundleName(path);//debuging
        LoadedAssetBundle lab = LoadAssetBundle(GetAssetBundleName(path));
        if (lab != null)
        {
            return lab.assetBundle.LoadAllAssets()[0];
        }

        return null;
    }

    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        LoadedAssetBundle lab = LoadAssetBundle(GetAssetBundleName(path));
        if (lab != null)
        {
            return lab.assetBundle.LoadAllAssets<T>()[0];
        }
        return null;
    }

    public static T Load<T>(string path, string name) where T : Object
    {
        LoadedAssetBundle lab = LoadAssetBundle(GetAssetBundleName(path));
        if (lab != null)
        {
            return lab.assetBundle.LoadAsset<T>(name);
        }

        return null;
    }
    
    static LoadedAssetBundle LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
    {
        if(!isLoadingAssetBundleManifest && _assetBundleManifest == null)
        {
            Initialize();
        }

        return LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);
    }

    static LoadedAssetBundle LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
    {
        //check exist
        string path = Application.persistentDataPath + "/assetbundle/" + assetBundleName;
        if( !File.Exists(path))//assetbundle 있어야할 폴더에 없으면 null 리턴
        {
            return null;
        }

        LoadedAssetBundle bundle = null;
        _loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if(bundle != null)
        {
            bundle.referencedCount++;
            return bundle;
        }

        try
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            if(assetBundle != null)
            {
                bundle = new LoadedAssetBundle(assetBundle);
                _loadedAssetBundles.Add(assetBundleName, bundle);

                //load dependencies
                if(!isLoadingAssetBundleManifest)
                {
                    string[] dependencies = _assetBundleManifest.GetDirectDependencies(assetBundleName);
                    for(int i=0; i<dependencies.Length; ++i)
                    {
                        LoadAssetBundleInternal(dependencies[i], false);
                    }
                }

//#if UNITY_EDITOR
//                Material[] materials = assetBundle.LoadAllAssets<Material>();
//                for(int i=0; i<materials.Length; ++i)
//                {
//                    Shader shader = Shader.Find(materials[i].shader.name);
//                    if(shader != null)
//                    {
//                        materials[i].shader = Shader.Find(materials[i].shader.name);
//                    }
//                }
//#endif
            }
        }
        catch( System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return bundle;
    }
    
    public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName)
    {
        LoadedAssetBundle bundle = null;
        _loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        return bundle;
    }
    
    public static void UnloadAssetBundle(string assetBundleName, bool unloadAllLoadedObjects = false)
    {
        UnloadAssetBundleInternal(GetAssetBundleName(assetBundleName), unloadAllLoadedObjects);
    }

    private static void UnloadAssetBundleInternal(string assetBundleName, bool unloadAllLoadedObjects)
    {
        LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName);
        if(bundle == null) { return; }

        if(--bundle.referencedCount == 0)
        {
            string[] dependencies = _assetBundleManifest.GetDirectDependencies(assetBundleName);
            for( int i=0; i<dependencies.Length; ++i )
            {
                UnloadAssetBundleInternal(dependencies[i], unloadAllLoadedObjects);
            }

            bundle.assetBundle.Unload(unloadAllLoadedObjects);
            _loadedAssetBundles.Remove(assetBundleName);
        }
    }

    //로딩된 에셋번들 키값 리스트 얻기
    public static List<string> GetLoadedAssetBundles()
    {
        List<string> assetList = new List<string>();
        assetList.AddRange(_loadedAssetBundles.Keys);
        return assetList;
    }






}
