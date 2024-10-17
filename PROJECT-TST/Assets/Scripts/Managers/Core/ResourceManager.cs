using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager 
{
    public Dictionary<string, Object> _resources = new Dictionary<string, Object>(); 

    public T Load<T>(string path) where T : Object
    {
        string name = path;
        int index = name.IndexOf('/');
        if (index > 0)
            name = name.Substring(index + 1);

        if (_resources.TryGetValue(name, out Object obj) == false)
        {
            T resource = Resources.Load<T>(path);
            _resources.Add(name, resource);

            return resource;
        }
        else
        {
            return _resources[name] as T;
        }
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject origin = Load<GameObject>($"Prefabs/{path}");
        if (origin == null)
        {

            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(origin, parent);
        go.name = origin.name;

        return go;
    }
}
