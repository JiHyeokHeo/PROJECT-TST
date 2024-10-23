using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Defines.Define;

public abstract class BaseController : InitBase
{
    // 여기에 추후 Scriptable 데이터 필요

    protected Creature _owner;
    protected Rigidbody _rigidbody;
    float _sqrMoveMagnitude = 0.0f;
    float _sqrinputMagnitude = 0.0f;

    public Creature Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public float SqrMoveManitude
    {
        get { return _sqrMoveMagnitude; }
        set { _sqrMoveMagnitude = value; }
    }

    public float SqrInputMagnitude
    {
        get { return _sqrinputMagnitude; }
        set { _sqrinputMagnitude = value; }
    }

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

    public virtual void SetInfo() { }

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
