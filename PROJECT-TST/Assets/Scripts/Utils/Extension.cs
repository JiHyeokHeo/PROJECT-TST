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

    public static void BindActions(this InputAction act, Action<InputAction.CallbackContext> action, EInputActionType actionType)
    {
        switch (actionType)
        {
            case EInputActionType.Performed:
                act.performed += action;
                break;
            case EInputActionType.Canceled:
                act.performed += action;
                break;

        }
    }

}
