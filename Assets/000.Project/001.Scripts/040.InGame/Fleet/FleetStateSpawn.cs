using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetStateSpawn : BaseState
    {
        private FleetAIController _aiController;
        private Fleet _fleet;

        public bool IsGoMove { get; private set; }
        
        public FleetStateSpawn(AIController controller) : base(controller)
        {
            _aiController = (FleetAIController)controller;
            _fleet = _aiController.GetComponent<Fleet>();
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);
            //Debug.Log("FleetStateSpawn / OnStateEnter");

            _fleet.shipList.Clear();
            Transform shipGroupObject = _fleet.transform.Find("Ships");
            int count = _fleet.fleetInfo.shipInfoList.Count;
            for (int i = 0; i < count; ++i)
            {
                Ship ship = (Utility.Instantiate("ObjectPrefab/Ship/" + _fleet.fleetInfo.shipInfoList[i].name + "_" + _fleet.fleetInfo.shipInfoList[i].grade) as GameObject).GetComponent<Ship>();
                Utility.SetParent(shipGroupObject, ship.transform, false);
                ship.SetShipInfo(_fleet.fleetInfo.shipInfoList[i]);
                _fleet.shipList.Add(ship);
            }

            this.SetTransition(_aiController.move);
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            //Debug.Log("FleetStateSpawn / OnStateUpdate");

        }

        public override void OnStateExit(State to, ITransition via)
        {
            base.OnStateExit(to, via);
            //Debug.Log("FleetStateSpawn / OnStateExit");
        }

    }
}


