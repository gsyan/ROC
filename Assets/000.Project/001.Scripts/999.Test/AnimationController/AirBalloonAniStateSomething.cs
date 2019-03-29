using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class AirBalloonAniStateSomething : BaseState
    {
        private AirBalloonAniController aIController;

        public AirBalloonAniStateSomething(AIController controller) : base(controller)
        {
            this.aIController = (AirBalloonAniController)controller;
        }


    }
}

