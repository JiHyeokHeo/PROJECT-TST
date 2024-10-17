using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public interface IMoveAble
{
    void Move();
}


public class TestScript : MonoBehaviour, IMoveAble
{
    List<string> _strings = new List<string>();

    private void Start()
    {
        string name = "ToryisPlayer";
        string[] names = name.Split("1");

        //if (names.Length < 0)
        //    return;

        foreach (string s in names) 
        {
            Debug.Log($"{s}");
        }

        Debug.Log($"{name}");
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {

    }
}
