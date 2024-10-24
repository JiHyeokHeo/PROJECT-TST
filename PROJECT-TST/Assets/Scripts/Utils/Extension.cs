using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using static Defines.Define;
using UnityEngine.InputSystem;

public static class Extension 
{
    public static T GetorAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go) 
    {
        return go != null && go.activeSelf;
    }

}
