using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    private string panelName;
    
    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        GInfo.SetupTest();

        BKST.UISystemBK.Instance.SetInputState(true);

        //BKST.UISystemBK.Instance.ShowPanel("Panel Asset");
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel Asset");
            panelName = "Panel Asset";
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel BattleMenu");
            panelName = "Panel BattleMenu";
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel Connecting");
            panelName = "Panel Connecting";
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel Loading");
            panelName = "Panel Loading";
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel Login");
            panelName = "Panel Login";
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel ManageFleet");
            panelName = "Panel ManageFleet";
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            BKST.UISystemBK.Instance.ShowPanel("Panel MessageBox");
            panelName = "Panel MessageBox";
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            BKST.UISystemBK.Instance.HidePanel(panelName, false);
        }

    }


}
