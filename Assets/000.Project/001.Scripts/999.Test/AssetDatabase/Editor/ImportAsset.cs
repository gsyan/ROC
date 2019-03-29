using UnityEngine;
using UnityEditor;

public class ImportAsset
{
    /// <summary>
    /// Unity는 일반적으로 에셋이 프로젝트에 드래그됐을 때 자동적으로 임포트하지만
    /// 스크립트 컨트롤로 임포트할 수도 있습니다.
    /// 그렇게 하려면 아래의 예처럼 AssetDatabase.ImportAsset 메서드를 사용합니다.
    /// </summary>
    [MenuItem("AssetDatabase/ImportExample")]
    static void ImportExample()
    {
        AssetDatabase.ImportAsset(GlobalValues.LAUNCHER_RESOURCES_TEXTURE +  "/black.png", ImportAssetOptions.Default);
    }


    /// <summary>
    /// 에디터는 에셋이 씬에 추가되거나 인스펙터 패널에서 편집된 경우 등 필요한 때만 에셋을 로드. 하지만 
    /// AssetDatabase.LoadAssetAtPath, AssetDatabase.LoadMainAssetAtPath, 
    /// AssetDatabase.LoadAllAssetRepresentationsAtPath, AssetDatabase.LoadAllAssetsAtPath
    /// 를 사용한 스크립트로 에셋을 로드 및 액세스할 수 있습니다. 자세한 내용은 스크립팅 문서를 참조하십시오.
    /// </summary>
    [MenuItem("AssetDatabase/LoadAssetExample")]
    static void LoadAssetExample()
    {
        Texture2D t = AssetDatabase.LoadAssetAtPath(GlobalValues.LAUNCHER_RESOURCES_TEXTURE + "/black.png", typeof(Texture2D)) as Texture2D;
    }


















}
