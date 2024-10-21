using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IController
{
    void SetInfo();
    void Move();
}

public class PlayerController : InitBase, IController
{
    Creature _owner;
    Rigidbody _rigidbody;

    float moveSpeed = 10.0f;
    Vector3 _moveDir;

    public Creature Owner
    {
        get { return _owner; } 
        set { _owner = value; }
    }

    public InputActionAsset inputActionAsset;
    private InputAction moveAction;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        moveAction = GetComponent<PlayerInput>().actions["Move"];

        // 추후 액션 관리 매니저가 필요해보임
        //moveAction = inputActionAsset.FindAction("Inputs/PlayerControls/Move", throwIfNotFound: true);
        moveAction.Enable();

        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        return true;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveDir = context.ReadValue<Vector2>();
        Debug.Log("Move input: " + _moveDir);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Movement stopped.");
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    public void SetInfo()
    {
        Owner = GetComponent<Player>();

        if (Owner != null)
            _rigidbody = Owner.GetOrAddComponent<Rigidbody>();
        
    }

    //#region LegacyMoveCode
    //public void Move()
    //{

    //    if (Owner == null)
    //    {
    //        Debug.Log("Owner is Missing");
    //        return;
    //    }

    //    Debug.Log(Owner.RigidBody.velocity.magnitude);
    //    // AddForce는 물리 업데이트에서 처리되어야 함
    //    Vector3 force = Vector3.zero;

    //    // 입력에 따른 힘 적용
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        force += new Vector3(-_force, 0, 0);
    //    }

    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        force += new Vector3(_force, 0, 0);
    //    }

    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        force += new Vector3(0, 0, _force);
    //    }

    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        force += new Vector3(0, 0, -_force);
    //    }

    //    // 힘을 최대값으로 제한
    //    force = ClampForce(force, _maxForce);

    //    // 제한된 힘을 Rigidbody에 적용
    //    Owner.RigidBody.AddRelativeForce(force, ForceMode.VelocityChange);
    //}
    //#endregion

    public void Move()
    {
        _rigidbody.velocity = new Vector3(_moveDir.x * moveSpeed, 0,  _moveDir.y * moveSpeed);
    }

    //private Vector3 ClampForce(Vector3 force, float maxForce)
    //{
    //    if (force.magnitude > maxForce)
    //    {
    //        return force.normalized * maxForce;
    //    }
    //    return force;
    //}
}
