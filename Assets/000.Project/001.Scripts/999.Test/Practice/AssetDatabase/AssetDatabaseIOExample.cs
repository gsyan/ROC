using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetDatabaseIOExample
{

    /// <summary>
    /// Unity는 에셋 파일의 메타 데이터를 유지하므로 파일 시스템을 사용해 생성, 이동 또는 삭제를 해서는 절대 안 됩니다.
    /// 대신 AssetDatabase.Contains, AssetDatabase.CreateAsset, AssetDatabase.CreateFolder,
    /// AssetDatabase.RenameAsset, AssetDatabase.CopyAsset, AssetDatabase.MoveAsset,
    /// AssetDatabase.MoveAssetToTrash, AssetDatabase.DeleteAsset
    /// 을 사용할 수 있습니다.
    /// </summary>
    [MenuItem ("AssetDatabase/FileOperationsExample")]
    static void Example()
    {
        string ret;

        //create
        Material material = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");
        if (AssetDatabase.Contains(material))
        {
            Debug.Log("Material asset created Already");
        }


        // Rename
        ret = AssetDatabase.RenameAsset("Assets/MyMaterial.mat", "MyMaterialNew");
        if (!string.IsNullOrEmpty(ret))
        {
            Debug.Log(ret);
        }


        // Create a Folder
        if (!Directory.Exists("Assets/NewFolder"))
        {
            ret = AssetDatabase.CreateFolder("Assets", "NewFolder");
            if (AssetDatabase.GUIDToAssetPath(ret) != "")
                Debug.Log("Folder asset created");
            else
                Debug.Log("Couldn't find the GUID for the path");
        }


        // Move
        ret = AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(material), "Assets/NewFolder/MyMaterialNew.mat");
        if (ret == "")
            Debug.Log("Material asset moved to NewFolder/MyMaterialNew.mat");
        else
            Debug.Log(ret);


        // Copy
        if (AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(material), "Assets/MyMaterialNew.mat"))
        {
            Debug.Log("Material asset copied as Assets/MyMaterialNew.mat");
        }
        else
        {
            Debug.Log("Couldn't copy the material");
        }

        // Manually refresh the Database to inform of a change
        AssetDatabase.Refresh();


        Material MaterialCopy = AssetDatabase.LoadAssetAtPath("Assets/MyMaterialNew.mat", typeof(Material)) as Material;


        // Move to Trash
        if (AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(MaterialCopy)))
            Debug.Log("MaterialCopy asset moved to trash");


        // Delete
        if (AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(material)))
            Debug.Log("Material asset deleted");
        if (AssetDatabase.DeleteAsset("Assets/NewFolder"))
            Debug.Log("NewFolder deleted");


        // Refresh the AssetDatabase after all the changes
        // 에셋의 수정이 완료되면 AssetDatabase.Refresh를 호출해야 변경사항이 데이터베이스에 커밋되고 프로젝트에 표시됩니다.
        AssetDatabase.Refresh();


    }

    
}
