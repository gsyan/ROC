using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private ShipInfo _shipInfo;

    public Transform[] beemGuns;
    public Transform[] missleGuns;

    [HideInInspector]
    public Vector3 positionCur;

    [HideInInspector]
    public Ship targetShip;

    private float _beamCoolTime = 0.0f;
    private float _missleCoolTime = 0.0f;

    


    private void Awake()
    {
        positionCur = Vector3.zero;
    }

    void Start () {
		
	}

    public void SetShipInfo(ShipInfo si)
    {
        _shipInfo = si;
    }

	
	void Update () {
		
	}

    public void MoveToNextLocalPosition(Vector3 nextPosition, float changingPercent)
    {
        transform.localPosition = Vector3.Lerp(positionCur, nextPosition, changingPercent);
    }
    public void SetLocalPosition(Vector3 nextPosition)
    {
        transform.localPosition = nextPosition;
        positionCur = nextPosition;
    }

    public void Attack()
    {
        _beamCoolTime -= Time.deltaTime;
        if( _beamCoolTime < 0.0f )
        {
            _beamCoolTime = _shipInfo.battleInfo.beamCool;

            

        }


    }



}
