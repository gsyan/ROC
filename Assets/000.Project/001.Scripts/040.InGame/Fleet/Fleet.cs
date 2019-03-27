using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.sbp.ai;

public class Fleet : CharacterBK
{
    public FleetInfo fleetInfo;
    
    public TeamType teamType;

    [HideInInspector]
    public List<Ship> shipList = new List<Ship>();
    private Vector3[] _shipsPositionNext;
        
    public List<Fleet> fleetList;             // 스테이지에 등장한 모든 함대 리스트 참조값
    public Fleet targetFleet = null;          // 타겟으로 잡은 함대

    private FleetAIController _aiController;


    public void SetInfo(FleetInfo info)
    {
        fleetInfo = info;
    }
    public void SetFleetList(ref List<Fleet> inFleetList)//타겟 검색을 위해 스테이지의 전체 함대 리스트를 가지고 있는다.
    {
        fleetList = inFleetList;
    }
    public void SetTeamType(TeamType type)
    {
        teamType = type;
    }
    public void SetSpawnPosition(Transform spawnTransform)
    {
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
    }



    protected override void Awake()
    {
        base.Awake();
        _aiController = gameObject.GetComponent<FleetAIController>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _aiController = null;
    }

    protected override void Start()
    {
        base.Start();
        if (_aiController != null)
        {
            _aiController.StartAI();
        }
    }

    protected override void Update()
    {
        base.Update();

    }





}



