using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITogglePage : UIToggleDefault
{

    public GameObject page;


    public override void Awake()
    {
        base.Awake();

        if (page != null)
        {
            page.SetActive(false);
        }
    }


    public override void UpdateState(bool state)
    {
        base.UpdateState(state);

        if (page != null)
        {
            page.SetActive(state);
        }
    }
}
