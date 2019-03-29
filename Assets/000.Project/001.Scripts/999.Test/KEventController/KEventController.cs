using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class KEventController : AIController
    {
        new public void StartAI()
        {
            State state_1_1 = new KE_1_1(this);
            //State state_1_2 = new KE_1_2(this);
            //State state_1_3 = new KE_1_3(this);
            //State state_1_4 = new KE_1_4(this);
            //State state_1_5 = new KE_1_5(this);

            fsm.AddState(state_1_1);

            base.StartAI();
        }

        protected virtual BaseState GetInterupptibleState()
        {
            BaseState ret = default(BaseState);

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("AirBalloonAniController / GetInterupptibleState if space");
            }
            return ret;
        }

    }
}

    
