using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetStateMove : BaseState
    {
        private FleetAIController _aiController;

        public bool IsGoDisappear { get; private set; }

        public FleetStateMove(AIController controller) : base(controller)
        {
            _aiController = (FleetAIController)controller;
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);
            Debug.Log("FleetStateMove / OnStateEnter");

        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            //Debug.Log("FleetStateMove / OnStateUpdate");
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                IsGoDisappear = true;
            }
        }

        public override void OnStateExit(State to, ITransition via)
        {
            base.OnStateExit(to, via);
            Debug.Log("FleetStateMove / OnStateExit");
        }

    }
}
