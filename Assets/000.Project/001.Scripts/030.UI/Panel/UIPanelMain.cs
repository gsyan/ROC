using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class UIPanelMain : UIPanelBase
{
    private LayeredPrefab[] _layeredPrefabs;

    public SceneLayerType currentLayer;
    
    public override void Awake()
    {
        base.Awake();

        _layeredPrefabs = GetComponentsInChildren<LayeredPrefab>();
    }

    public void ChangeLayer(SceneLayerType newLayer, bool skipTween = false)
    {
        int fCount = _layeredPrefabs.Length;
        for (int i = 0; i < fCount; ++i)
        {
            //상황에 따라서 활성화를 생량할 것들을 제외   ///////////////////
#if !USE_IAP
            if (string.Compare(_layeredPrefabs[i].prefab.name, "Regular Market") == 0 ||
                string.Compare(_layeredPrefabs[i].prefab.name, "Package market") == 0
                )
            { continue; }
#endif
#if !SOCIAL_GOOGLE_FACEBOOK
            if (string.Compare(_layeredPrefabs[i].prefab.name, "Google Service") == 0)
            { continue; }
#endif
            ///////////////////////////////////////////////////////////////

            _layeredPrefabs[i].ChangeLayer(newLayer, skipTween);
        }

        currentLayer = newLayer;
    }

    public void Deactive()
    {
        int fCount = _layeredPrefabs.Length;
        for(int i = 0; i < fCount; ++i)
        {
            _layeredPrefabs[i].Deactive();
        }
        currentLayer = SceneLayerType.None;
    }
    
    public void RemoveOnSceneLoad()
    {
        int fCount = _layeredPrefabs.Length;
        for(int i = 0; i< fCount; ++i)
        {
            _layeredPrefabs[i].RemoveOnSceneLoad();
        }
    }

    //public void SetupPlayer(ClientActor actor)
    //{
    //    Transform tm = Utility.SearchActive(transform, GlobalValues.PLAYER_GRAPHIC_STATUS);
    //    if(tm != null)
    //    {
    //        //내용 작성해야 함

    //    }
    //}











    public void SetupPause(string resumeText = "", string exitText = "", Callback onExit = null)
    {
        Transform tm = Utility.SearchActive(transform, "Button Pause");
        if (tm != null)
        {
            tm.GetComponent<UIButtonPause>().Setup(resumeText, exitText, onExit);
        }
    }



}

