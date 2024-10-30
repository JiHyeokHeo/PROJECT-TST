using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

namespace TST
{
    //유니티 공식 Document ObjectPool
    //https://unity.com/kr/how-to/use-object-pooling-boost-performance-c-scripts-unity
    //https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Pool.ObjectPool_1.html
    class Pool
    {
        GameObject prefab;
        IObjectPool<GameObject> objectPool;

        public Pool(GameObject prefab)
        {
            this.prefab = prefab;
            // bool check, Capacity, maxSize = default값으로 설정
            objectPool = new ObjectPool<GameObject>(CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        public void Push(GameObject prefab)
        {
            if (prefab.activeSelf) 
                objectPool.Release(prefab);
        }

        public GameObject Pop()
        {
            return objectPool.Get();
        }

        public void Clear()
        {
            
        }

        #region Unity SampleCode
        // invoked when creating an item to populate the object pool
        //private RevisedProjectile CreateProjectile()
        //{
        //    RevisedProjectile projectileInstance = Instantiate(projectilePrefab);
        //    projectileInstance.ObjectPool = objectPool;
        //    return projectileInstance;
        //}

        //// invoked when returning an item to the object pool
        //private void OnReleaseToPool(RevisedProjectile pooledObject)
        //{
        //    pooledObject.gameObject.SetActive(false);
        //}

        //// invoked when retrieving the next item from the object pool
        //private void OnGetFromPool(RevisedProjectile pooledObject)
        //{
        //    pooledObject.gameObject.SetActive(true);
        //}

        //// invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        //private void OnDestroyPooledObject(RevisedProjectile pooledObject)
        //{
        //    Destroy(pooledObject.gameObject);
        //}
        #endregion
        // ObjectPool 내부 구현
        #region ObjectPoolInternalFunc

        private GameObject CreateObject()
        {
            GameObject instance = GameObject.Instantiate(this.prefab);
            instance.name = this.prefab.name;

            return instance;
        }

        private void OnGetFromPool(GameObject pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(GameObject pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(GameObject pooledObject)
        {
            GameObject.Destroy(pooledObject.gameObject);
        }
        #endregion
    }


    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance { get; private set; }

        // 이펙트 데이터 연동 드래그드롭 임시 
        // 게임 엔진 툴의 강점을 살리기 위해 드래그드롭이 오히려 좋은가? @_@ 모르게똬.. 추후 질문
        public List<EffectBase> effects;

        private List<GameObject> activeEffects = new List<GameObject>();
        private Dictionary<string, Pool> pools = new Dictionary<string, Pool> ();

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime; 

            for (int i = 0; i < activeEffects.Count; i++)
            {
                GameObject activeEffect = activeEffects[i];
                EffectBase effectBase = activeEffect.GetComponent<EffectBase>();
                if (effectBase == null)
                {
                    // 이건 아무리 봐도 시간 복잡도가 N^2 인데에에에에으이에으에에 // 그냥 제거를 빼버리고 마지막에 특정 갯수가 쌓였을 때 Clear를 한다? 
                    activeEffects.Remove(activeEffect); 
                    return;
                }
                // 0초가 되면 true 반환
                if (effectBase.UpdateEffectBase(deltaTime))
                {
                    pools[activeEffects[i].name].Push(activeEffects[i]);
                    activeEffects.Remove(activeEffects[i]); 
                }
            }
        }

        private void Push(GameObject go)
        {
            if (pools.ContainsKey(go.name) == false)
                CreatePool(go);
           
            pools[go.name].Push(go);
        }

        private GameObject Pop(string name)
        {
            if (pools.ContainsKey(name) == false)
            {
                Debug.Log($"Failed to Pop {name}");
                return null;
            }
            
            return pools[name].Pop();
        }

        public GameObject SpawnEffect(GameObject go, Vector3 pos, Quaternion rotation)
        {
            if (pools.ContainsKey(go.name) == false)
                Push(go);

            GameObject popObj = Pop(go.name);
            if (popObj == null)
                return null;

            activeEffects.Add(popObj);
            popObj.transform.SetPositionAndRotation(pos, rotation);
            return popObj;
        }

        private void CreatePool(GameObject go)
        {
            Pool pool = new Pool(go);

            pools.Add(go.name, pool);
        }
    }
}
