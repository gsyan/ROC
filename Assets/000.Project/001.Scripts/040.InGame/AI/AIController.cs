using UnityEngine;

namespace com.sbp.ai
{
    public class AIController : MonoBehaviour 
    {
        protected FiniteStateMachine fsm = new FiniteStateMachine();
        
        private void Update()
        {
            if( fsm != null )
            {
                fsm.UpdateState();
            }
        }     

        public virtual void StartAI()
        {
            if( fsm != null )
            {
                fsm.StartFSM();
            }
        }

        

    }
}