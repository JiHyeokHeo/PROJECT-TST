using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IController
{
    void SetInfo();
    void Move();
}

public class PlayerController : InitBase, IController
{
    float _force = 1.0f;
    float _maxForce = 1.0f;
    Creature _owner;
    Vector3 _inputVec;
    Vector3 _moveVec;

    public Creature Owner
    {
        get { return _owner; } 
        set { _owner = value; }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        return true;
    }

    public void SetInfo()
    {
        Owner = GetComponent<Player>();
    }

    #region LegacyMoveCode
    public void Move()
    {

        if (Owner == null)
        {
            Debug.Log("Owner is Missing");
            return;
        }

        Debug.Log(Owner.RigidBody.velocity.magnitude);
        // AddForce는 물리 업데이트에서 처리되어야 함
        Vector3 force = Vector3.zero;

        // 입력에 따른 힘 적용
        if (Input.GetKey(KeyCode.A))
        {
            force += new Vector3(-_force, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            force += new Vector3(_force, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            force += new Vector3(0, 0, _force);
        }

        if (Input.GetKey(KeyCode.S))
        {
            force += new Vector3(0, 0, -_force);
        }

        // 힘을 최대값으로 제한
        force = ClampForce(force, _maxForce);

        // 제한된 힘을 Rigidbody에 적용
        Owner.RigidBody.AddRelativeForce(force, ForceMode.VelocityChange);
    }
    #endregion

    //public void Move()
    //{
    //    //_moveVec = 
    //}

    void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }

    private Vector3 ClampForce(Vector3 force, float maxForce)
    {
        if (force.magnitude > maxForce)
        {
            return force.normalized * maxForce;
        }
        return force;
    }
}
