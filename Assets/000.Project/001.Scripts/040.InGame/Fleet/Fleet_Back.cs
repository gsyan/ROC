using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Fleet_Back : MonoBehaviour
{
    private FleetInfo _fleetInfo;

    private TeamType _teamType;

    private bool _bChaingingFormation = false;

    [HideInInspector]
    public List<Ship> shipList = new List<Ship>();
    private Vector3[] _shipsPositionNext;


    private List<Fleet_Back> _fleetList;             // 스테이지에 등장한 모든 함대 리스트 참조값
    private Fleet_Back _targetFleet = null;          // 타겟으로 잡은 함대
    
	
    public void SetFleetInfo(FleetInfo info)
    {
        _fleetInfo = info;
    }
    public void SetFleetList(ref List<Fleet_Back> fleetList)//타겟 검색을 위해 스테이지의 전체 함대 리스트를 가지고 있는다.
    {
        _fleetList = fleetList;
    }

    public void SetTeamType(TeamType type)
    {
        _teamType = type;
    }
    public void SetSpawnPosition(Transform spawnTransform)
    {
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
    }


    void Start ()
    {
        shipList.Clear();
        Transform shipGroupObject = transform.Find("Ships");
        int count = _fleetInfo.shipInfoList.Count;
        for (int i=0; i<count; ++i)
        {
            Ship ship = (Utility.Instantiate("ObjectPrefab/Ship/" + _fleetInfo.shipInfoList[i].name + "_" + _fleetInfo.shipInfoList[i].grade) as GameObject).GetComponent<Ship>();
            Utility.SetParent(shipGroupObject, ship.transform, false);
            ship.SetShipInfo(_fleetInfo.shipInfoList[i]);
            shipList.Add(ship);
            //if ( i < _fleetInfo.shipInfoList.Count )
            //{
            //    Ship ship1 = shipGroupObject.GetChild(i).GetComponent<Ship>();
            //    shipList.Add(ship);
            //}
            //else
            //{
            //    shipGroupObject.GetChild(i).gameObject.SetActive(false);
            //}

        }

        _bChaingingFormation = true;
        ChaingeFormation(_fleetInfo.formationType);


    }
	
	
	void Update () {

        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //Debug.Log("onScreen: " + onScreen + " / x: " + screenPoint.x + " / y: " + screenPoint.y + " / z: " + screenPoint.z);

        FindTargetFleet();
        Move();
    }

    private void FindTargetFleet()
    {
        if(_targetFleet != null) { return; }

        int count = _fleetList.Count;
        float distance = -1;
        for(int i=0; i< count; ++i)
        {
            if (_fleetList[i]._teamType == this._teamType)
            {
                continue;
            }

            float d = Vector3.Distance(transform.position, _fleetList[i].transform.position);
            if( distance < 0 || distance > d )
            {
                distance = d;
                _targetFleet = _fleetList[i];
            }
        }
    }
    private void Move()
    {
        if( _targetFleet != null )
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
        float distance = Vector3.Distance(transform.position, _targetFleet.transform.position);
        if ( distance > _fleetInfo.battleInfo.attackRange )
        {
            Vector3 direction = Vector3.Normalize(_targetFleet.transform.position - transform.position);
            transform.position = transform.position + direction * _fleetInfo.battleInfo.moveSpeed * Time.deltaTime;
        }

    }

    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _targetFleet.transform.position);
        if (distance < _fleetInfo.battleInfo.attackRange)
        {
            for(int i=0; i< shipList.Count; ++i)
            {
                shipList[i].targetShip = FindTargetShip_CloseFirst(shipList[i]);
                shipList[i].Attack();
            }
        }
    }
    private Ship FindTargetShip_CloseFirst(Ship ship)
    {
        Ship targetShip = null;
        float distance = -1;
        for (int i=0; i< _targetFleet.shipList.Count; ++i)
        {
            float d = Vector3.Distance(ship.transform.position, _targetFleet.shipList[i].transform.position);
            if (distance < 0 || distance > d)
            {
                distance = d;
                targetShip = _targetFleet.shipList[i];
            }
        }
        return targetShip;
    }
    


    public void ChaingeFormation(FleetFormationType type)
    {
        StartCoroutine(ChaingingFormation(type));
    }
    private IEnumerator ChaingingFormation(FleetFormationType type)
    {
        _shipsPositionNext = GData.Instance.GetShipPosition(type);

        if ( _fleetInfo.formationType != type || _bChaingingFormation == true)
        {
            _bChaingingFormation = true;
            float defaultChangingTime = 3.0f; //진형변경 기본 시간 3초
            float realChangingTime = defaultChangingTime / _fleetInfo.battleInfo.moveSpeed;
            float sec = 0;  // sec 에 deltaTime 이 더해져 changingPercent(sec/realChangingTime)가 1이 되면 완료
            float changingPercent = 0.0f;
            while (_bChaingingFormation)
            {
                sec += Time.deltaTime;
                for (int i = 0; i < shipList.Count; ++i)
                {
                    changingPercent = sec / realChangingTime;
                    shipList[i].MoveToNextLocalPosition(_shipsPositionNext[i], changingPercent);
                    if (changingPercent > 1.0f)
                    {
                        for (int k = 0; k < shipList.Count; ++k)
                        {
                            shipList[k].SetLocalPosition(_shipsPositionNext[k]);
                        }
                        _bChaingingFormation = false;
                        _fleetInfo.formationType = type;
                        break;
                    }
                }
                yield return null;
            }
        }
        else
        {
            for (int i = 0; i < shipList.Count; ++i)
            {
                shipList[i].SetLocalPosition(_shipsPositionNext[i]);
            }
            yield return null;
        }

        
    }



}


