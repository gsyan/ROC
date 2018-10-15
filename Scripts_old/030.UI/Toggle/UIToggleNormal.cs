using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleNormal : UIToggleExtension
{
    protected UISprite checkMark;

    protected List<EventDelegate> onUpdateState = new List<EventDelegate>();

    public override void UpdateState(bool state)
    {
        //base.UpdateState(state);
        if( checkMark != null )
        {
            checkMark.enabled = state;
        }

        EventDelegate.Execute(onUpdateState);
    }
}
