using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class KE_1_5 : BaseState
    {
        private KEventController _eventController;
        private AirBalloonAniController _aniController;


        public KE_1_5(AIController controller) : base(controller)
        {
            _eventController = (KEventController)controller;
            _aniController = _eventController.GetComponent<AirBalloonAniController>();
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);


        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();

            //string stepName = _aniController.aniSteps[_aniController.aniStepIndex].stepName;


            //this.SetTransition()



        }


    }
}
    
