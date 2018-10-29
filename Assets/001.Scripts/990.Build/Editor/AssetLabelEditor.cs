using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AssetLabelEditor : EditorWindow
{
    private const float WINDOW_WIDTH = 500.0f;
    private const float WINDOW_HEIGHT = 600.0f;
    private const float LABEL_WIDTH = 100.0f;

    private string[] assetBundleNames;
    private string searchText = "";
    private List<string> searchList = new List<string>();
    private int selectedIndex = 0;
    private string selectedBundleName = "";

    private string rename = "";
    private Vector2 scrollPosition;


    [MenuItem("ROC/AssetLabel Editor", false, 3)]
    public static void ShowWindow()
    {
        AssetLabelEditor editor = (AssetLabelEditor)EditorWindow.GetWindow(typeof(AssetLabelEditor));
        editor.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
    }

    private void OnEnable()
    {
        assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
    }

    private void OnGUI()
    {
        EditorGUIUtility.labelWidth = LABEL_WIDTH;

        // SEARCH
        string newSearchText = EditorGUILayout.TextField("Search:", searchText);
        if(newSearchText != searchText)
        {
            searchList.Clear();
            for(int i=0; i<assetBundleNames.Length; ++i)
            {
                if (assetBundleNames[i].Contains(searchText))
                {
                    searchList.Add(assetBundleNames[i]);
                }
            }

            searchText = newSearchText;
            selectedIndex = 0;
        }

        // Asset Labels
        EditorGUILayout.BeginHorizontal();
        {
            selectedBundleName = "";

            if(string.IsNullOrEmpty(searchText))
            {
                selectedIndex = EditorGUILayout.Popup("Asset Labels", selectedIndex, assetBundleNames);
                if( selectedIndex < assetBundleNames.Length )
                {
                    selectedBundleName = assetBundleNames[selectedIndex];
                }
            }
            else
            {
                selectedIndex = EditorGUILayout.Popup("Asset Labels", selectedIndex, searchList.ToArray());
                if(selectedIndex < searchList.Count)
                {
                    selectedBundleName = searchList[selectedIndex];
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        // CHANGE LABEL
        GUILayout.Space(5);
        rename = EditorGUILayout.TextField("Rename:", rename);

        string[] assetFiles = AssetDatabase.GetAssetPathsFromAssetBundle(selectedBundleName);
        if (GUILayout.Button("Change Name"))
        {
            for (int i = 0; i < assetFiles.Length; i++)
            {
                AssetImporter importer = AssetImporter.GetAtPath(assetFiles[i]);
                importer.assetBundleName = rename;
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
            Refresh(rename);
        }

        if( GUILayout.Button("Remove UnusedAssetBundleNames"))
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            Refresh(selectedBundleName);
        }

        // ASSET LIST
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Asset List");

        EditorGUI.indentLevel++;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 180));

        for (int i = 0; i < assetFiles.Length; i++)
        {
            EditorGUILayout.LabelField(string.Format("[{0:00}] {1}", i + 1, assetFiles[i]));
        }

        EditorGUILayout.EndScrollView();
        EditorGUI.indentLevel--;


    }





    void Refresh(string selectName)
    {
        selectedIndex = 0;

        assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

        searchList.Clear();

        if (string.IsNullOrEmpty(searchText))
        {
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                if (assetBundleNames[i] == selectName)
                {
                    selectedIndex = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                if (assetBundleNames[i].Contains(searchText))
                {
                    searchList.Add(assetBundleNames[i]);
                }
            }

            for (int i = 0; i < searchList.Count; i++)
            {
                if (searchList[i] == selectName)
                {
                    selectedIndex = i;
                    break;
                }
            }
        }
    }
    
}
