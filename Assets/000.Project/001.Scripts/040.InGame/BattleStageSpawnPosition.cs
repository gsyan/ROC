using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStageSpawnPosition : MonoBehaviour
{
    public Transform[] _spawnPointBlue;
    public Transform[] _spawnPointRed;
    public Transform[] _spawnPointGreen;

    
    public Transform[] GetSpawnPosition(TeamType type)
    {
        switch(type)
        {
            case TeamType.Blue:
                return _spawnPointBlue;
            case TeamType.Red:
                return _spawnPointRed;
            case TeamType.Green:
                return _spawnPointGreen;
        }
        return null;
    }

    




}
