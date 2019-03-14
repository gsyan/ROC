using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawnTime : MonoBehaviour {

    public string poolName;
    public float time;
    public bool ignoreTimeScale = true;

    private float elapsedTime = 0.0f;


    void OnSpawned()
    {
        elapsedTime = 0.0f;
    }


    void Update()
    {
        elapsedTime += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        if (elapsedTime >= time)
        {
            BKST.PoolManager.GetPool(poolName).Despawn(transform);
        }
    }
}
