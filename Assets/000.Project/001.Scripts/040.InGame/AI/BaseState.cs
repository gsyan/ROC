using UnityEngine;

namespace com.sbp.ai
{
    public abstract class BaseState : State
    {
        protected AIController controller {get; private set;}

        private BaseState(){}
        
        public BaseState(AIController controller)
        {
            this.controller = controller;
        }

        public override void OnStateEnter (State from, ITransition via)
        {
        }

        public override void OnStateUpdate ()
        {

        }

        public override void OnStateExit (State to, ITransition via)
        {
        }

        public override void Dispose ()
        {
            base.Dispose();
        }
    }
}
