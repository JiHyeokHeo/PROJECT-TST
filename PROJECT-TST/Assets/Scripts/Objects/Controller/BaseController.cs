using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Defines.Define;

public abstract class BaseController : InitBase
{
    // ���⿡ ���� Scriptable ������ �ʿ�

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
    protected abstract void OnEnable();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }


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

    public void BindActions(InputAction act, Action<InputAction.CallbackContext> action, EInputActionType actionType)
    {
        switch (actionType)
        {
            case EInputActionType.Performed:
                act.performed += action;
                break;
            case EInputActionType.Canceled:
                act.canceled += action;
                break;
            case EInputActionType.None:
                break;

        }
    }

    protected void EnableAllActions()
    {
        if (_activeActions.Count == 0)
            return;

        foreach (InputAction act in _activeActions) 
        {
            act.Enable();
        }
    }

    protected void DisableAllActions()
    {
        if (_activeActions.Count == 0)
            return;

        foreach (var act in _activeActions)
        {
            act.Disable();
        }
    }
}
