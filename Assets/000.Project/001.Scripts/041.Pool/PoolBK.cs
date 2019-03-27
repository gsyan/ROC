using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BKST
{
    /// <summary>
    /// scene 최초 오브젝트에 컴퍼넌트 되어 있는 상태를 추천
    /// PoolSystem 를 통해 풀에서 관리하는 자원을 부르고 소멸시킨다
    /// </summary>
    [AddComponentMenu("BKST/PoolManager/PoolBK")]
    public sealed class PoolBK : MonoBehaviour
    {
        #region Inspector Parameters
        public string poolName = "";
        public bool matchPoolScale = false;
        public bool matchPoolLayer = false;
        public Transform poolObject;//부모로 쓸 별도의 오브젝트 없으면 현재의 오브젝트가 부모가 됨
        public bool _dontDestroyOnLoad = false;
        public float maxParticleDespawnTime = 300.0f;
        #endregion Inspector Parameters



        #region Private Properties
        private Dictionary<string, Transform> _prefabDic = new Dictionary<string, Transform>();//해당 prefab의 poolOfPrefab이 없어, 처음 만들때 여기 저장
        private List<PoolOfPrefab> _poolOfPrefabList = new List<PoolOfPrefab>();//poolOfPrefab 리스트
        private List<Transform> _activeList = new List<Transform>();//활성 상태의 프리팹 저장
        #endregion Private Properties

        #region Constructor and Init
        private void Awake()
        {
            if (poolObject == null)
            {
                poolObject = transform;
            }

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(poolObject.gameObject);
            }

            if (poolName == "")
            {   
                poolName = poolObject.name.Replace("Pool", "");
                poolName = poolName.Replace("(Clone)", "");
            }

            PoolManager.Add(this);
        }
        private void OnDestroy()
        {
            PoolManager.Remove(this);

            StopAllCoroutines();

            _activeList.Clear();

            foreach(PoolOfPrefab pop in _poolOfPrefabList)
            {
                pop.SelfDestruct();//destroy object
            }
            _poolOfPrefabList.Clear();

            _prefabDic.Clear();

            //오늘의 할일 : 윗줄 PoolManager.Remove 내부 부터 완성
        }



        #endregion Constructor and Init

        #region Spawn Function
        public bool IsSpawned(Transform instance)
        {
            return _activeList.Contains(instance);
        }
        public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
        {
            return Spawn(prefab, pos, rot, null);
        }
        public Transform Spawn(Transform prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }
        public Transform Spawn(Transform prefab, Transform parent)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
        }
        public Transform Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            return Spawn(prefab.transform, pos, rot);
        }
        public Transform Spawn(GameObject prefab)
        {
            return Spawn(prefab.transform);
        }
        public Transform Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab.transform, parent);
        }
        public Transform Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            return Spawn(prefab.transform, pos, rot, parent);
        }
        public Transform Spawn(string prefabName)
        {
            Transform prefab = _prefabDic[prefabName];
            return Spawn(prefab);
        }
        public Transform Spawn(string prefabName, Transform parent)
        {
            Transform prefab = _prefabDic[prefabName];
            return Spawn(prefab, parent);
        }
        public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot)
        {
            Transform prefab = _prefabDic[prefabName];
            return Spawn(prefab, pos, rot);
        }
        public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform prefab = this._prefabDic[prefabName];
            return this.Spawn(prefab, pos, rot, parent);
        }
        public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform instance = null;
            
            #region Reuse
            for(int i=0; i< _poolOfPrefabList.Count; ++i)
            {
                if(_poolOfPrefabList[i].prefabGO == prefab.gameObject )
                {
                    instance = _poolOfPrefabList[i].SpawnInstance(pos, rot);
                    if (instance == null) return null;
                    if (parent != null)
                    {
                        instance.parent = parent;
                    }
                    else
                    {
                        instance.parent = poolObject;
                    }

                    _activeList.Add(instance);

                    instance.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);

                    return instance;
                }
            }
            #endregion Reuse

            #region MakeNew
            PoolOfPrefab pop = new PoolOfPrefab(prefab);
            CreatePoolOfPrefab(pop);

            instance = pop.SpawnInstance(pos, rot);
            if (parent != null)
            {
                instance.parent = parent;
            }
            else
            {
                instance.parent = poolObject;
            }

            _activeList.Add(instance);
            instance.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
            return instance;
            #endregion MakeNew
        }
        private void CreatePoolOfPrefab(PoolOfPrefab poolOfPrefab)
        {
            bool isAlreadyPool = this.GetPoolOfPrefab(poolOfPrefab.prefabGO) == null ? false : true;
            if(!isAlreadyPool)
            {
                poolOfPrefab.pool = this;
                _poolOfPrefabList.Add(poolOfPrefab);
                _prefabDic.Add(poolOfPrefab.prefab.name, poolOfPrefab.prefab);
            }

            if(poolOfPrefab.bPreLoaded != true)
            {
                poolOfPrefab.PreloadInstances();
            }
        }
        #endregion Spawn Function

        #region Spawn AudioSource Function
        public AudioSource Spawn(AudioSource prefab, Vector3 pos, Quaternion rot)
        {
            return Spawn(prefab, pos, rot, null);
        }
        public AudioSource Spawn(AudioSource prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }
        public AudioSource Spawn(AudioSource prefab, Transform parent)
        {
            return this.Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
        }
        public AudioSource Spawn(AudioSource prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform instance = Spawn(prefab.transform, pos, rot, parent);
            if (instance == null) { return null; }

            AudioSource aud = instance.GetComponent<AudioSource>();
            aud.Play();

            StartCoroutine(ListForAudioStop(aud));

            return aud;
        }
        private IEnumerator ListForAudioStop(AudioSource src)
        {
            yield return null;

            while(src.isPlaying)
            {
                yield return null;
            }

            Despawn(src.transform);
        }
        #endregion Spawn AudioSource Function

        #region Spawn Particle Function
        public ParticleSystem Spawn(ParticleSystem prefab, Vector3 pos, Quaternion rot)
        {
            return Spawn(prefab, pos, rot, null);
        }
        public ParticleSystem Spawn(ParticleSystem prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform instance = Spawn(prefab.transform, pos, rot, parent);
            if (instance == null)
            {
                return null;
            }

            ParticleSystem emitter = instance.GetComponent<ParticleSystem>();
            StartCoroutine(ListenForEmitDespawn(emitter));
            return emitter;
        }
        private IEnumerator ListenForEmitDespawn(ParticleSystem emitter)
        {
            yield return new WaitForSeconds(emitter.startDelay + 0.25f);
            float safeTimer = 0.0f;
            while(emitter.IsAlive(true))
            {
                if(!emitter.gameObject.activeInHierarchy)
                {
                    emitter.Clear(true);
                    yield break;
                }

                safeTimer += Time.deltaTime;
                if(safeTimer > maxParticleDespawnTime)
                {

                }
                yield return null;
            }

            Despawn(emitter.transform);
        }
        #endregion Spawn Particle Function

        #region Despawn
        public void Despawn(Transform instance, Transform parent)
        {
            instance.parent = parent;
            Despawn(instance);
        }
        public void Despawn(Transform instance, float seconds)
        {
            StartCoroutine(DespawnAfterSeconds(instance, seconds, null));
        }
        public void Despawn(Transform instance, float seconds, Transform parent)
        {
            StartCoroutine(DespawnAfterSeconds(instance, seconds, parent));
        }
        public void DespawnAll()
        {
            //아래의 복사 과정을 거치지 않아도 잘 작동 하는지 확인 필요
            //List<Transform> spawned = new List<Transform>(this._spawnedList);

            for (int i = 0; i < _activeList.Count; ++i)
            {
                Despawn(_activeList[i]);
            }
        }
        public void Despawn(Transform instance)
        {
            bool despawned = false;
            for(int i = 0; i < _poolOfPrefabList.Count; ++i)
            {
                if(_poolOfPrefabList[i].activeList.Contains(instance))
                {
                    despawned = _poolOfPrefabList[i].DespawnInstance(instance);
                    break;
                }
                else if(_poolOfPrefabList[i].deactiveQueue.Contains(instance))
                {
                    return;
                }
            }

            if (!despawned)
            {
                return;
            }
            
            _activeList.Remove(instance);
        }
        private IEnumerator DespawnAfterSeconds(Transform instance, float seconds, Transform parent)
        {
            GameObject go = instance.gameObject;
            while(seconds > 0)
            {
                yield return 0;
                if(!go.activeInHierarchy)
                {
                    yield break;
                }
                seconds -= Time.deltaTime;
            }

            if (parent != null)
            {
                Despawn(instance, parent);
            }
            else
            {
                Despawn(instance);
            }
        }
        #endregion Despawn

        #region Utility
        public PoolOfPrefab GetPoolOfPrefab(GameObject prefab)
        {
            for(int i = 0; i < _poolOfPrefabList.Count; ++i)
            {
                if(_poolOfPrefabList[i].prefabGO == prefab )
                {
                    return _poolOfPrefabList[i];
                }
            }
            return null;
        }
        public PoolOfPrefab GetPoolOfPrefab(Transform xform)
        {
            for (int i = 0; i < _poolOfPrefabList.Count; ++i)
            {
                if (_poolOfPrefabList[i].prefabGO == xform.gameObject)
                {
                    return _poolOfPrefabList[i];
                }
            }
            return null;
        }
        public GameObject GetPrefab(GameObject instance)
        {
            for( int i = 0; i < _poolOfPrefabList.Count; ++i)
            {
                if( _poolOfPrefabList[i].Contains(instance.transform) )
                {
                    return _poolOfPrefabList[i].prefabGO;
                }
            }
            return null;
        }
        public GameObject GetPrefab(Transform instance)
        {
            for (int i = 0; i < _poolOfPrefabList.Count; ++i)
            {
                if (_poolOfPrefabList[i].Contains(instance))
                {
                    return _poolOfPrefabList[i].prefabGO;
                }
            }
            return null;
        }

        #endregion Utility


    }
    /////////////////////////////////////////////////////////////////
    [System.Serializable]
    public class PoolOfPrefab
    {
        public Transform prefab;
        public GameObject prefabGO;

        //속한 풀
        public PoolBK pool;

        //active 상태인 것을 모아둔 리스트
        public List<Transform> activeList = new List<Transform>();
        //deactive 상태인 것을 모아둔 리스트
        //public List<Transform> deactiveList = new List<Transform>();
        public Queue<Transform> deactiveQueue = new Queue<Transform>();

        public int totalCount
        {
            get
            {
                int count = 0;
                count = activeList.Count + deactiveQueue.Count;
                return count;
            }
        }

        public bool bPreLoaded = false;
        private int _preloadAmount = 0;

        private int _limitAmount = 100;



        public PoolOfPrefab(Transform prefab)
        {
            this.prefab = prefab;
            this.prefabGO = prefab.gameObject;
        }
        public PoolOfPrefab() { }

        public void SelfDestruct()
        {
            prefab = null;
            prefabGO = null;
            pool = null;

            foreach(Transform form in activeList)
            {
                if( form != null)
                {
                    Object.Destroy(form.gameObject);
                }
            }
            activeList.Clear();
            foreach (Transform form in deactiveQueue)
            {
                if (form != null)
                {
                    Object.Destroy(form.gameObject);
                }
            }
            deactiveQueue.Clear();
        }


        internal Transform SpawnInstance(Vector3 pos, Quaternion rot)
        {
            Transform instance = null;

            if(deactiveQueue.Count == 0 )//there's no unused prefab
            {
                instance = SpawnNew(pos, rot);//instantiate new prefab
            }
            else//there is atleast more than one prefab
            {
                instance = deactiveQueue.Dequeue();
                activeList.Add(instance);

                if(instance == null)
                {
                    throw new MissingReferenceException("Don't delete despawned instance directly");
                }
                else
                {
                    instance.position = pos;
                    instance.rotation = rot;
                    instance.gameObject.SetActive(true);
                }
            }
            return instance;
        }
        public Transform SpawnNew()
        {
            return SpawnNew(Vector3.zero, Quaternion.identity);
        }
        public Transform SpawnNew(Vector3 pos, Quaternion rot)
        {
            //limit case work to do

            if (pos == Vector3.zero) pos = pool.poolObject.position;
            if (rot == Quaternion.identity) rot = pool.poolObject.rotation;

            Transform instance = Object.Instantiate(prefab, pos, rot);
            ChangeName(instance);


            activeList.Add(instance);

            return instance;
        }
        private void ChangeName(Transform instance)
        {
            instance.name = instance.name.Replace("(Clone)", "");
            instance.name += (totalCount + 1).ToString("#0000");
        }
        public void PreloadInstances()
        {
            if (bPreLoaded || prefab == null)
            {
                return;
            }

            if (_preloadAmount >= _limitAmount)
            {
                _preloadAmount = _limitAmount;
            }

            //추가 작업 필요

        }

        public bool DespawnInstance(Transform xform)
        {
            return DespawnInstance(xform, true);
        }
        public bool DespawnInstance(Transform xform, bool sendEventMessage)
        {
            bool res = false;
            res = activeList.Remove(xform);
            deactiveQueue.Enqueue(xform);

            if (sendEventMessage)
            {
                xform.gameObject.BroadcastMessage("OnDespawned", pool, SendMessageOptions.DontRequireReceiver);
            }

            xform.gameObject.SetActive(false);

            return res;
        }

        public bool Contains(Transform xform)
        {
            if(prefabGO == null) { return false; ; }

            bool res = false;
            res = activeList.Contains(xform);
            if( res )
            {
                return true;
            }
            res = deactiveQueue.Contains(xform);
            if( res )
            {
                return true;
            }

            return false;
        }
        



    }


}