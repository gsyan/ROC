using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetStateSpawn : BaseState
    {
        private FleetAIController _aiController;

        public bool IsGoMove { get; private set; }
        
        public FleetStateSpawn(AIController controller) : base(controller)
        {
            _aiController = (FleetAIController)controller;
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);
            Debug.Log("FleetStateSpawn / OnStateEnter");
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            //Debug.Log("FleetStateSpawn / OnStateUpdate");


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                IsGoMove = true;
            }
            
        }

        public override void OnStateExit(State to, ITransition via)
        {
            base.OnStateExit(to, via);
            Debug.Log("FleetStateSpawn / OnStateExit");
        }

    }
}


