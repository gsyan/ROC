using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        BKST.UISystem.Instance.SetInputState(true);

        //BKST.UISystem.Instance.ShowPanel("Panel Asset");

        string filePath = Application.dataPath.Replace("Assets", string.Empty) + "TestData" + ".txt";
        Utility.CreateDirectory(filePath);
        string msg = "";
        using (StreamWriter sw = File.CreateText(filePath))
        {
            msg = "1234";
            sw.WriteLine("{0}", msg);
            sw.Flush();
        }

        StartCoroutine(TestF());
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            BKST.UISystem.Instance.ShowPanel("Panel Asset");
            panelName = "Panel Asset";
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            BKST.UISystem.Instance.ShowPanel("Panel BattleMenu");
            panelName = "Panel BattleMenu";
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            BKST.UISystem.Instance.ShowPanel("Panel Connecting");
            panelName = "Panel Connecting";
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            BKST.UISystem.Instance.ShowPanel("Panel Loading");
            panelName = "Panel Loading";
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            BKST.UISystem.Instance.ShowPanel("Panel Login");
            panelName = "Panel Login";
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            BKST.UISystem.Instance.ShowPanel("Panel ManageFleet");
            panelName = "Panel ManageFleet";
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            BKST.UISystem.Instance.ShowPanel("Panel MessageBox");
            panelName = "Panel MessageBox";
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            BKST.UISystem.Instance.HidePanel(panelName, false);
        }


        

    }

    float startTime;
    float latency;

    private void FixedUpdate()
    {
        startTime = Time.realtimeSinceStartup;

        //latency = Time.realtimeSinceStartup - startTime;
        //startTime = Time.realtimeSinceStartup;
        //Debug.Log("latancy: " + latency);

    }
    IEnumerator TestF()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();

            latency = Time.realtimeSinceStartup - startTime;
            Debug.Log("latancy: " + latency);
        }
            

        
    }



}
