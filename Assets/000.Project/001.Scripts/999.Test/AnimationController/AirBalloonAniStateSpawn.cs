using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class AirBalloonAniStateSpawn : BaseState
    {
        private AirBalloonAniController _aniController;
        
        public bool IsGoing { get; private set; }


        public AirBalloonAniStateSpawn(AIController controller) : base(controller)
        {
            _aniController = (AirBalloonAniController)controller;
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);

            IsGoing = _aniController.bAutoStart;

        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsGoing = true;
            }
        }

    }
}


