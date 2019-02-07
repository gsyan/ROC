using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPanelBattleMenu : UIPanelBase
{
    

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnActive()
    {
        base.OnActive();

    }

    public override void OnDeactive()
    {
        base.OnDeactive();
    }






    public void EnterBattleStage()
    {
        //SceneLoad.nextScene = "BattleStage" + id.ToString();
        SceneLoad.nextScene = "BattleStage";
        SceneManager.LoadScene("loading");
    }










    public void OnCloseButton()
    {
        UISystem.Instance.HidePanel(this.transform, false);
    }
}
