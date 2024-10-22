using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Defines.Define;
using static UnityEditor.PlayerSettings;

public interface IController
{
    void SetInfo();
    void Move();
}

public class PlayerController : BaseController, IController
{
    protected float _moveSpeed = 10.0f;
    GameObject _followTarget;
    Vector2 _mousePos = Vector2.zero;
    Vector2 _look = Vector2.zero;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _followTarget = Util.FindChild(gameObject, "FollowTarget", true);
        _followTarget.transform.SetPositionAndRotation(new Vector3(0, 1, -3), Quaternion.identity); 
        SetPlayerInputAction("Move");
        EnableAction("Move");
        GetPlayerInputAction("Move").BindActions((context) =>
        {
            _moveDir = context.ReadValue<Vector2>();
      

        }, EInputActionType.Performed);

        return true;
    }

    // InputSystem 활용
    void OnRotation(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        Vector3 targetVelocity = new Vector3(_moveDir.x * _moveSpeed, _rigidbody.velocity.y, _moveDir.y * _moveSpeed);
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, targetVelocity, Time.deltaTime * _moveSpeed);

        // 델타 값이 있을 때만 회전 처리
        if (mouseDelta != Vector2.zero)
        {
            //X축은 수평 회전(yaw), Y축은 수직 회전(pitch)
            _look.x += mouseDelta.x * 500.0f * Time.deltaTime;
            _look.y -= mouseDelta.y * 500.0f * Time.deltaTime;

            // 회전 적용 (Euler 방식) 
            // 카메라 전환을 위해 플레이어 x축만 고정, _followTarget x,y축 자유)
            _followTarget.transform.rotation = Quaternion.Euler(-_look.y, _look.x, 0);
            transform.rotation = Quaternion.Euler(0, _look.x, 0);
        }
    }
    protected override void OnDisable()
    {
        GetPlayerInputAction("Move").Disable();
    }

    public void SetInfo()
    {
        Owner = GetComponent<Player>();

        if (Owner != null)
            _rigidbody = Owner.GetOrAddComponent<Rigidbody>();
    }

    
    public void Move()
    {
        Vector3 movePos = new Vector3(_moveDir.x, 0, _moveDir.y);

        Owner.SetPosition(movePos, _moveSpeed);
    }

    public Vector3 _targetVelocity = Vector3.zero;

}
