using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetStateDisappear : BaseState
    {
        private FleetAIController _aiController;

        public bool IsReadyForSpawn { get; private set; }

        public FleetStateDisappear(AIController controller) : base(controller)
        {
            _aiController = (FleetAIController)controller;
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);
            Debug.Log("FleetStateDisappear / OnStateEnter");

            IsReadyForSpawn = false;

            _aiController.StartCoroutine(Wait());
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            //Debug.Log("FleetStateDisappear / OnStateUpdate");
        }

        public override void OnStateExit(State to, ITransition via)
        {
            base.OnStateExit(to, via);
            Debug.Log("FleetStateDisappear / OnStateExit");
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);

            _aiController.gameObject.SetActive(false);
            IsReadyForSpawn = true;
        }

    }
}