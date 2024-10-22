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
    [SerializeField]
    protected float _moveSpeed = 5.0f;
    [SerializeField]
    protected float AngleLimit = 60.0f;
    

    GameObject _followTarget;
   
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _followTarget = Util.FindChild(gameObject, "FollowTarget", true);
        _followTarget.transform.position = new Vector3(0, 1, -3); 

        SetPlayerInputAction("Move");
        EnableAction("Move");
        BindActions(GetPlayerInputAction("Move"), (context) =>
        {
            _moveDir = context.ReadValue<Vector2>();
        }, EInputActionType.Performed);

        return true;
    }

    // InputSystem Ȱ��
    Vector2 _look = Vector2.zero;
    void OnRotation(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        // ��Ÿ ���� ���� ���� ȸ�� ó��
        if (mouseDelta != Vector2.zero)
        {
            //X���� ���� ȸ��(yaw), Y���� ���� ȸ��(pitch)
            _look.x += mouseDelta.x * 250.0f * Time.deltaTime;
            _look.y -= mouseDelta.y * 250.0f * Time.deltaTime;

            _look.y = Mathf.Clamp(_look.y, -AngleLimit, AngleLimit);
            // ȸ�� ���� (Euler ���) 
            // ī�޶� ��ȯ�� ���� �÷��̾� x�ุ ����, _followTarget x,y�� ����)
            _followTarget.transform.rotation = Quaternion.Euler(-_look.y, _look.x, 0);
            transform.rotation = Quaternion.Euler(0, _look.x, 0);
        }
    }

    protected override void OnEnable()
    {
        // ����, ���ε��� �̺�Ʈ �ϴ� Enable
        EnableAllActions();
    }

    protected override void OnDisable()
    {
        // ����, ���ε��� �̺�Ʈ �ϴ� Disable
        DisableAllActions();
    }

    public void SetInfo()
    {
        Owner = GetComponent<Player>();

        if (Owner != null)
            _rigidbody = Owner.GetOrAddComponent<Rigidbody>();
    }

    
    public void Move()
    {
        Vector3 moveDirLocal = new Vector3(_moveDir.x, 0, _moveDir.y);

        Owner.SetPositionByLocalDirection(moveDirLocal, 5.0f);
    }

    public Vector3 _targetVelocity = Vector3.zero;

}
