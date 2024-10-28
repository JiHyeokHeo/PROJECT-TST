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
        Transform rootTransform;
        GameObject prefab;
        IObjectPool<GameObject> objectPool;

        public Pool(GameObject prefab, Transform root)
        {
            this.prefab = prefab;
            rootTransform = root;
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
            instance.transform.parent = this.rootTransform;

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

        public List<EffectBase> effects;

        private Dictionary<string, Pool> pools = new Dictionary<string, Pool> ();

        private void Awake()
        {
            Instance = this;
            //effects.Add(effectPrefab1);
            //effects.Add(effectPrefab2);
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

            popObj.transform.SetPositionAndRotation(pos, rotation);

            return popObj;
        }

        public void PushEffectToPool(GameObject go)
        {
            Push(go);
        }

        private void CreatePool(GameObject go)
        {
            // 이펙트 매니저 트랜스폼 밑에 붙도록 설정
            Pool pool = new Pool(go, this.transform);

            pools.Add(go.name, pool);
        }
    }
}
