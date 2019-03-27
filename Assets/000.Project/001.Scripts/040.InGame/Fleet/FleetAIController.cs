using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetAIController : AIController
    {
        private Fleet _fleet;

        public State spawn;
        public State move;
        private State _disappear;


        public override void StartAI()
        {
            _fleet = gameObject.GetComponent<Fleet>();

            spawn = new FleetStateSpawn(this);
            move = new FleetStateMove(this);
            _disappear = new FleetStateDisappear(this);


            spawn
                .SetTransition(move, () => ((FleetStateSpawn)spawn).IsGoMove);

            move
                .SetTransition(GetInterupptibleState);


            fsm.AddState(spawn);
            fsm.AddState(move);
            fsm.AddState(_disappear);


            base.StartAI();
        }


        protected virtual BaseState GetInterupptibleState()
        {
            BaseState ret = default(BaseState);

            if (Input.GetKey(KeyCode.Alpha3))
            {
                return (BaseState)_disappear;
            }

            return ret;
        }

    }
}

