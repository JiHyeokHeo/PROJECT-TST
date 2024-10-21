using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Defines.Define;

public abstract class BaseController : InitBase
{
    protected Creature _owner;
    protected Rigidbody _rigidbody;

    public Creature Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    protected Vector3 _moveDir;

    protected InputActionAsset _inputActionAsset;
    protected Dictionary<string, InputAction> _playerKeyBoardInputDic = new Dictionary<string, InputAction>();
    protected List<InputAction> _activeActions = new List<InputAction>();

    protected abstract void OnDisable();

    protected InputAction SetPlayerInputAction(string name, EInputType type = EInputType.KeyBoard)
    {
        if (_playerKeyBoardInputDic.ContainsKey(name))
            return null;

        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            return null;

        if (type == EInputType.KeyBoard)
        {
            _playerKeyBoardInputDic.Add(name, playerInput.actions[$"{name}"]);
        }

        return playerInput.actions[$"{name}"];
    }

    protected InputAction GetPlayerInputAction(string name, EInputType type = EInputType.KeyBoard) 
    {
        if (_playerKeyBoardInputDic.TryGetValue(name, out InputAction action) == false)
        {
            Debug.Log("Failed to Get InputAction");
            return null;
        }

        return action;
    }

    protected void EnableAction(string name)
    {
        InputAction action = GetPlayerInputAction(name);
        if (action == null)
            return;

        _activeActions.Add(GetPlayerInputAction(name));
        action.Enable();
    }
}
