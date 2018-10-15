using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKST
{
    public static class PoolManager
    {
        private static readonly Dictionary<string, PoolBK> _poolDic = new Dictionary<string, PoolBK>();
        public static void Add(PoolBK pool)
        {
            if(ContainKey(pool.poolName))
            {
                return;
            }

            _poolDic.Add(pool.poolName, pool);
        }
        public static bool Remove(PoolBK pool)
        {
            if(!_poolDic.ContainsKey(pool.poolName) & Application.isPlaying)
            {
                return false;
            }
            _poolDic.Remove(pool.poolName);
            return false;
        }
        public static bool Remove(string poolName)
        {
            throw new System.NotImplementedException("Not Implemented");

            if (!_poolDic.ContainsKey(poolName) & Application.isPlaying)
            {
                return false;
            }
            _poolDic.Remove(poolName);
            return false;
        }
        public static int Count
        {
            get
            {
                return _poolDic.Count;
            }
        }
        public static bool ContainKey(string poolName)
        {
            return _poolDic.ContainsKey(poolName);
        }
        public static bool TryGetValue(string poolName, out PoolBK pool)
        {
            return _poolDic.TryGetValue(poolName, out pool);
        }
        public static PoolBK GetPool(string poolName)
        {
            PoolBK p = null;
            _poolDic.TryGetValue(poolName, out p);
            return p;
        }
    }

}

