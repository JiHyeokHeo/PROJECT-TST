using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Defines.Define;
using static UnityEditor.PlayerSettings;
using static HARDCODING;

public interface IController
{
    // �� ��Ʈ�ѷ��� Owner����
    void SetInfo();
    void Move();
}

public class PlayerController : BaseController, IController
{
    GameObject _followTarget;
    Vector2 _look = Vector2.zero;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _followTarget = Util.FindChild(gameObject, "FollowTarget", true);
        _followTarget.transform.position = new Vector3(0, 1, -3);

        // 0 �� TPS ī�޶�, 1�� FPS ī�޶�, 2�� �� ī�޶� ��� etc ī�޶� �Ŵ����� �ʿ��ҵ�? �ӽ÷� ���� �Ŵ���
        Managers.GameManager.SetCameraTarget<TPSCamera>(gameObject);

        SetPlayerInputAction("Move");
        EnableAction("Move");
        BindActions(GetPlayerInputAction("Move"), (context) =>
        {
            _moveDir = context.ReadValue<Vector2>();
        }, EInputActionType.Performed);

        return true;
    }

    public void SetInfo()
    {
        Owner = GetComponent<Player>();
    }

    // InputSystem Ȱ�� 
    void OnRotation(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        // ��Ÿ ���� ���� ���� ȸ�� ó��
        if (mouseDelta != Vector2.zero)
        {
            //X���� ���� ȸ��(yaw), Y���� ���� ȸ��(pitch)
            _look.x += mouseDelta.x * ROTATIONSPEEDTEMP * Time.deltaTime;
            _look.y -= mouseDelta.y * ROTATIONSPEEDTEMP * Time.deltaTime;

            _look.y = Mathf.Clamp(_look.y, -ANGLELIMITTEMP, ANGLELIMITTEMP);
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

    public void Move()
    {
        Vector3 moveDirLocal = new Vector3(_moveDir.x, 0, _moveDir.y);

        Owner.SetPositionByLocalDirection(moveDirLocal, 5.0f);
    }
}
