using UnityEngine;
using UnityEditor;          //EditorWindow

//using System.Collections;
using System.IO;


//이 스크립트는 editor 폴더에 넣어야 한다.
public partial class BuildTool : EditorWindow
{
    private const float WINDOW_WIDTH = 500.0f;
    private const float WINDOW_HEIGHT = 600.0f;
    private const float LABEL_WIDTH = 140.0f;

    //private const string ANDROID_DIR = "android";
    //private const string IOS_DIR = "ios";
    //private const string MANIFEST_DIR = "manifest";

    private const string LAUNCHER_RESOURCE_DIR = "Assets/050.Launcher/Resources";

    private const string RESOURCE_DIR = "Assets/Resources";
    private const string RESOURCE_TEMP_DIR = "Assets/Resources_";
    private const string RESOURCE_MEDIA_DIR = "Assets/Resources/Media";

    private const string STREAMING_DIR = "Assets/StreamingAssets";
    private const string STREAMING_TEMP_DIR = "Assets/StreamingAssets_";
    private const string STREAMING_MEDIA_DIR = "Assets/StreamingAssets/Media";

    private const string CSV_DIR = "Assets/Resources/CSV";
    private const string LUA_DIR = "Assets/StreamingAssets/LuaScripts";

    private const string CSV_ZIP_PATH = "Assets/csv.zip";
    private const string LUA_ZIP_PATH = "Assets/lua.zip";
    private const string CSV_PACK_PATH = RESOURCE_DIR + "/csv.bytes";
    private const string LUA_PACK_PATH = RESOURCE_DIR + "/lua.bytes";
    
    private string _rootDir;
    private string[] excludeExtensions = new string[] { ".meta", ".cs", ".js", ".xlsm", ".shader" };//제외 대상 확장자

    private int _selectedTab;

    public void OnEnable()
    {
        _rootDir = "D:/bk/Project/ROC/bin"; //PlayerPrefs.GetString("buildtool_root_dir");
        _selectedTab = PlayerPrefs.GetInt("buildtool_selected_tab", 0);
    }


    [MenuItem("ROC/Build", false, 10)]
    public static void ShowWindow()
    {
        BuildTool bt = (BuildTool)EditorWindow.GetWindow(typeof(BuildTool));
        //bk, 이 타이밍에 OnEnable() 수행됨
        bt.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
    }

    public void OnGUI()
    {
        _selectedTab = GUILayout.Toolbar(_selectedTab, new string[] { "PATCH", "APPLICATION" });
        PlayerPrefs.SetInt("buildtool_selected_tab", _selectedTab);
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.TextField("Root Dir", _rootDir);
            if (GUILayout.Button("...", GUILayout.Width(50)))
            {
                string newDir = EditorUtility.OpenFolderPanel("", _rootDir, "");
                if( !string.IsNullOrEmpty(newDir) )
                {
                    _rootDir = newDir;
                    //PlayerPrefs.SetString("buildtool_root_dir", _rootDir);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        switch(_selectedTab)
        {
            case 0:
                DrawPatchGUI();
                break;
            case 1:
                DrawApplicationGUI();
                break;
        }
    }

    private bool IsExcludeExtension(string path)//제외시킬 것 적용
    {
        int fCount = excludeExtensions.Length;
        for ( int i = 0; i < fCount; ++i )
        {
            if( path.EndsWith(excludeExtensions[i]) )
            {
                return true;
            }
        }

        return false;
    }
    


    private string GetAssetBundleName(string path)
    {
        return AssetBundleUtility.GetPathWithoutExtension(path).Replace(Path.AltDirectorySeparatorChar, '_');
    }




}
