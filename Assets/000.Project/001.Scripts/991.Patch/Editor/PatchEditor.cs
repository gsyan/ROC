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
                    //script.address = "http://192.168.0.21/BlackNight_FTP/";
                    //script.address = "http://volthole2.iptime.org:10011/patch/";
                    //address = "http://192.168.0.14:10011/patch/";//내부용
                    //address = "http://vrive.cdn3.cafe24.com/guardian_arena/patch/";//볼트홀 소유 외부용
                    script.address = "http://ga.cdn.3rdeyesys.com/guardian_arena/roc/patch/";
                    script.filePath = "android_test/";
                    EditorGUILayout.LabelField("Address", script.address);
                    EditorGUILayout.LabelField("FilePath", script.filePath);
                    break;

                case Patch.URLType.QA:
                    script.address = "http://ga.cdn.3rdeyesys.com/guardian_arena/roc/patch/";
                    script.filePath = "android_qa/";
                    EditorGUILayout.LabelField("Address", script.address);
                    EditorGUILayout.LabelField("FilePath", script.filePath);
                    break;

                case Patch.URLType.Live:
                    script.address = "http://ga.cdn.3rdeyesys.com/guardian_arena/roc/patch/";
                    script.filePath = "android_live/";
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
