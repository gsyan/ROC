using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BKST;

public static class PoolSystem
{
    public static void LoadUI(string name)
    {
        DespawnUI(SpawnUI(name));
    }
    public static void LoadParticle(string name)
    {
        DespawnParticle(SpawnParticle(name));
    }
    public static void LoadSound(string name)
    {
        DespawnSound(SpawnSound(name));
    }


    public static Transform SpawnUI(string name)
    {
        PoolBK pool;
        if (PoolManager.TryGetValue("GUI", out pool))
        {
            return Spawn(pool, "UI/" + name);
        }
        return null;
    }
    public static Transform SpawnParticle(string name)
    {
        PoolBK pool;
        if (PoolManager.TryGetValue("Particle", out pool))
        {
            return Spawn(pool, "Particle/" + name);
        }
        return null;
    }
    public static Transform SpawnSound(string name)
    {
        PoolBK pool;
        if (PoolManager.TryGetValue("Sound", out pool))
        {
            return Spawn(pool, "Sound/" + name);
        }
        return null;
    }
    public static Transform SpawnTutorialEffect(string name)
    {
        //SpawnPool spawnPool;
        //if (PoolManager.Pools.TryGetValue("TutorialEffect", out spawnPool))
        //{
        //    return Spawn(spawnPool, name);
        //}

        return null;
    }
    static Transform Spawn(PoolBK pool, string path)
    {
        GameObject prefab = ResourceSystem.Load(path) as GameObject;
        if (prefab != null)
        {
            PoolOfPrefab pop = pool.GetPoolOfPrefab(prefab);
            if (pop != null)
            {
                ResourceSystem.Unload(path);
            }
            else
            {
                ResourceSystem.RegisterReference(path, pool.gameObject);
            }

            return pool.Spawn(prefab.transform, prefab.transform.position, prefab.transform.rotation);
        }

        return null;
    }
    

    public static void DespawnUI(Transform tm)
    {
        if (tm != null && tm.gameObject.activeSelf)
        {
            PoolManager.GetPool("GUI").Despawn(tm);
        }
    }
    public static void DespawnParticle(Transform tm)
    {
        if (tm != null && tm.gameObject.activeSelf)
        {
            PoolManager.GetPool("Particle").Despawn(tm);
        }
    }
    public static void DespawnSound(Transform tm)
    {
        if (tm != null && tm.gameObject.activeSelf)
        {
            PoolManager.GetPool("Sound").Despawn(tm);
        }
    }

}
