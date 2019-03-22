using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class Fleet : MonoBehaviour
    {
        private FleetInfo _fleetInfo;

        private TeamType _teamType;

        private bool _bChaingingFormation = false;

        [HideInInspector]
        public List<Ship> shipList = new List<Ship>();
        private Vector3[] _shipsPositionNext;


        private List<Fleet_Back> _fleetList;             // 스테이지에 등장한 모든 함대 리스트 참조값
        private Fleet_Back _targetFleet = null;          // 타겟으로 잡은 함대



        private FleetAIController _aiController;
        
        private void Awake()
        {
            _aiController = gameObject.GetComponent<FleetAIController>();
        }

        private void Start()
        {
            if(_aiController != null)
            {
                _aiController.StartAI();
            }
        }





    }
}


