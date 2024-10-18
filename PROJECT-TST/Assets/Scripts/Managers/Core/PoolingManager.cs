using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

class Pool
{
    private GameObject _prefab;
    private IObjectPool<GameObject> _pool;

    private Transform _root;
    private Transform Root
    {
        get 
        {
            if (_root == null)
            {
                GameObject go = new GameObject() { name = $"@{_prefab.name}Pool" };
                _root = go.transform;
            }

            return _root;
        }
    }
  
    public Pool(GameObject prefab)
    {
        _prefab = prefab;
        _pool = new ObjectPool<GameObject>(CreateObject, OnGet, OnRelease, OnDestory);
    }

    public void Push(GameObject go)
    {
        if (go.activeSelf)
            _pool.Release(go);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    #region ObjectPool Func
    private GameObject CreateObject()
    {
        GameObject go = GameObject.Instantiate(_prefab);
        go.transform.SetParent(Root);
        go.name = _prefab.name;
        return go;
    }

    private void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    private void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }

    private void OnDestory(GameObject go)
    {
        GameObject.Destroy(go);
    }

    #endregion

}

public class PoolingManager  
{
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false)
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
    }

    public void Push(GameObject go)
    {
        if (_pools.ContainsKey(go.name) == false)
            Debug.Log($"{go.name} is Failed to Pool");

        _pools[go.name].Push(go);
    }

    public void Clear()
    {
        _pools.Clear();
    }


    private void CreatePool(GameObject prefab)
    {
        Pool pool = new Pool(prefab);
        _pools.Add(prefab.name, pool);
    }
}
