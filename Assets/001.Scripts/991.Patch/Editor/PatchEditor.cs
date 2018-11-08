using UnityEngine;
using UnityEditor;
using System.IO; //Directoty

[CanEditMultipleObjects]
[CustomEditor(typeof(Patch))]
public class PatchEditor : Editor
{
    private Patch script;

    private void OnEnable()
    {
        //스크립트 대입
        script = target as Patch;
    }

    //여기서 다뤄주지 않는 public property는 인스팩터창에 뜨지 않게 된다.
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        if (script != null)
        {
            script.urlType = (Patch.URLType)EditorGUILayout.EnumPopup("URL Type", script.urlType);//"URL Type" 인스팩터창의 항목 이름
            
            switch (script.urlType)
            {
                case Patch.URLType.Local:
                    string directory = Directory.GetCurrentDirectory();//현재 프로젝트 폴더 까지의 경로
                    directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);// '\\' 을 '/'으로 교체
                    directory = string.Format("{0}{1}/", "file://", directory);//경로 완성
                    script.address = PlayerPrefs.GetString("patch_local_url", directory);//patch.cs 의 address property 값
                    EditorGUILayout.LabelField("Address", script.address);

                    if(GUILayout.Button("Select Directory"))
                    {
                        string newUrl = EditorUtility.OpenFolderPanel( "", "", "");//탐색창에서 폴더 선택하면 해당 경로를 리턴
                        if (!string.IsNullOrEmpty(newUrl))
                        {
                            directory = string.Format("{0}{1}/", "file://", newUrl);
                            Debug.Log(directory);
                            PlayerPrefs.SetString("patch_local_url", directory);
                        }
                    }
                    
                    break;
                case Patch.URLType.Dev:
                    script.address = "https://cdn.jsdelivr.net/gh/gsyan/ROCPatch";
                    EditorGUILayout.LabelField("Address", script.address);

                    EditorGUILayout.BeginHorizontal();
                    script.bVersion = EditorGUILayout.Toggle("Version", script.bVersion);
                    script.verion = "@v" + Application.version;
                    EditorGUILayout.LabelField(script.verion + ".", new GUILayoutOption[] { GUILayout.MaxWidth(63) } );
                    script.patchVersion = EditorGUILayout.TextField(script.patchVersion, new GUILayoutOption[] { GUILayout.MaxWidth(40) });
                    EditorGUILayout.EndHorizontal();

                    script.filePath = "/patch/android_test/";
                    EditorGUILayout.LabelField("FilePath", script.filePath);

                    script.useAssetbundle = EditorGUILayout.Toggle("UseAssetbundle", script.useAssetbundle);

                    break;

                case Patch.URLType.QA:
                    script.address = "http://ga.cdn.3rdeyesys.com/guardian_arena/roc/patch/";
                    script.filePath = "/patch/android_qa/";
                    EditorGUILayout.LabelField("Address", script.address);
                    EditorGUILayout.LabelField("FilePath", script.filePath);
                    break;

                case Patch.URLType.Live:
                    script.address = "http://ga.cdn.3rdeyesys.com/guardian_arena/roc/patch/";
                    script.filePath = "/patch/android_live/";
                    EditorGUILayout.LabelField("Address", script.address);
                    EditorGUILayout.LabelField("FilePath", script.filePath);
                    break;
                    
                case Patch.URLType.Custom:
                    script.address = EditorGUILayout.TextField("Address", script.address);
                    EditorGUILayout.LabelField("FilePath", script.filePath);
                    break;
            }
            
        }
    }




}
