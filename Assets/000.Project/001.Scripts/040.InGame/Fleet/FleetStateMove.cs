using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetStateMove : BaseState
    {
        private FleetAIController _aiController;
        private Fleet _fleet;

        private bool _bChaingingFormation = true;
        private Vector3[] _shipsPositionNext;


        public FleetStateMove(AIController controller) : base(controller)
        {
            _aiController = (FleetAIController)controller;
            _fleet = _aiController.GetComponent<Fleet>();
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);
            //Debug.Log("FleetStateMove / OnStateEnter");
            ChaingeFormation(_fleet.fleetInfo.formationType);
        }
        public void ChaingeFormation(FleetFormationType type)
        {
            _aiController.StartCoroutine(ChaingingFormation(type));
        }
        private IEnumerator ChaingingFormation(FleetFormationType type)
        {
            _shipsPositionNext = GData.Instance.GetShipPosition(type);

            if (_fleet.fleetInfo.formationType != type || _bChaingingFormation == true)
            {
                _bChaingingFormation = true;
                float defaultChangingTime = 3.0f; //진형변경 기본 시간 3초
                float realChangingTime = defaultChangingTime / _fleet.fleetInfo.moveInfo.moveSpeed;
                float sec = 0;  // sec 에 deltaTime 이 더해져 changingPercent(sec/realChangingTime)가 1이 되면 완료
                float changingPercent = 0.0f;
                while (_bChaingingFormation)
                {
                    sec += Time.deltaTime;
                    for (int i = 0; i < _fleet.shipList.Count; ++i)
                    {
                        changingPercent = sec / realChangingTime;
                        _fleet.shipList[i].MoveToNextLocalPosition(_shipsPositionNext[i], changingPercent);
                        if (changingPercent > 1.0f)
                        {
                            for (int k = 0; k < _fleet.shipList.Count; ++k)
                            {
                                _fleet.shipList[k].SetLocalPosition(_shipsPositionNext[k]);
                            }
                            _bChaingingFormation = false;
                            _fleet.fleetInfo.formationType = type;
                            break;
                        }
                    }
                    yield return null;
                }
            }
            else
            {
                for (int i = 0; i < _fleet.shipList.Count; ++i)
                {
                    _fleet.shipList[i].SetLocalPosition(_shipsPositionNext[i]);
                }
                yield return null;
            }
        }




        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            //Debug.Log("FleetStateMove / OnStateUpdate");

            FindTargetFleet();
            Move();
        }
        private void FindTargetFleet()
        {
            if (_fleet.targetFleet != null) { return; }

            int count = _fleet.fleetList.Count;
            float distance = -1;
            for (int i = 0; i < count; ++i)
            {
                if (_fleet.fleetList[i].teamType == _fleet.teamType)
                {
                    continue;
                }

                float d = Vector3.Distance(_fleet.transform.position, _fleet.fleetList[i].transform.position);
                if (distance < 0 || distance > d)
                {
                    distance = d;
                    _fleet.targetFleet = _fleet.fleetList[i];
                }
            }
        }
        private void Move()
        {
            if (_fleet.targetFleet != null)
            {
                MoveToTarget();
                Attack();
            }
            else
            {

            }
        }
        private void MoveToTarget()
        {
            float distance = Vector3.Distance(_fleet.transform.position, _fleet.targetFleet.transform.position);
            if (distance > _fleet.fleetInfo.attackInfo.attackRange)
            {
                Vector3 direction = Vector3.Normalize(_fleet.targetFleet.transform.position - _fleet.transform.position);
                _fleet.transform.position = _fleet.transform.position + direction * _fleet.fleetInfo.moveInfo.moveSpeed * Time.deltaTime;
            }

        }
        private void Attack()
        {
            float distance = Vector3.Distance(_fleet.transform.position, _fleet.targetFleet.transform.position);
            if (distance < _fleet.fleetInfo.attackInfo.attackRange)
            {
                for (int i = 0; i < _fleet.shipList.Count; ++i)
                {
                    _fleet.shipList[i].targetShip = FindTargetShip_CloseFirst(_fleet.shipList[i]);
                    _fleet.shipList[i].Attack();
                }
            }
        }
        private Ship FindTargetShip_CloseFirst(Ship ship)
        {
            Ship targetShip = null;
            float distance = -1;
            for (int i = 0; i < _fleet.targetFleet.shipList.Count; ++i)
            {
                float d = Vector3.Distance(ship.transform.position, _fleet.targetFleet.shipList[i].transform.position);
                if (distance < 0 || distance > d)
                {
                    distance = d;
                    targetShip = _fleet.targetFleet.shipList[i];
                }
            }
            return targetShip;
        }






        public override void OnStateExit(State to, ITransition via)
        {
            base.OnStateExit(to, via);
            //Debug.Log("FleetStateMove / OnStateExit");
        }

    }
}
