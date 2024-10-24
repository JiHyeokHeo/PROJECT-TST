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
    // 각 컨트롤러에 Owner세팅
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

        // 0 번 TPS 카메라, 1번 FPS 카메라, 2번 줌 카메라 등등 etc 카메라 매니저가 필요할듯? 임시로 게임 매니저
        Managers.GameManager.SetCameraTarget<TPSCamera>(gameObject);

        SetPlayerInputAction("Move");
        EnableAction("Move");

        // 인풋에 관한 이벤트 함수
        BindActions(GetPlayerInputAction("Move"), (context) =>
        {
            Owner.Dir = context.ReadValue<Vector2>();
            SqrInputMagnitude += context.ReadValue<Vector2>().sqrMagnitude;
            Debug.Log(Owner.Dir);
        }, EInputActionType.Performed);

        // 플레이어 컨트롤 액션 Interaction Trigger Event 라이프 사이클 관련
        // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/Interactions.html
        BindActions(GetPlayerInputAction("Move"), (context) =>
        {
            SqrInputMagnitude = 0;
            Debug.Log(SqrInputMagnitude);
        }, EInputActionType.Canceled);
        return true;
    }

    public override void SetInfo()
    {
        Owner = GetComponent<Player>();
    }

    // InputSystem 활용 
    void OnRotation(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        // 델타 값이 있을 때만 회전 처리
        if (mouseDelta != Vector2.zero)
        {
            //X축은 수평 회전(yaw), Y축은 수직 회전(pitch)
            _look.x += mouseDelta.x * ROTATIONSPEEDTEMP * Time.deltaTime;
            _look.y -= mouseDelta.y * ROTATIONSPEEDTEMP * Time.deltaTime;

            _look.y = Mathf.Clamp(_look.y, -ANGLELIMITTEMP, ANGLELIMITTEMP);
            // 회전 적용 (Euler 방식) 
            // 카메라 전환을 위해 플레이어 x축만 고정, _followTarget x,y축 자유)
            _followTarget.transform.rotation = Quaternion.Euler(-_look.y, _look.x, 0);
            transform.rotation = Quaternion.Euler(0, _look.x, 0);
        }
    }

    protected override void OnEnable()
    {
        // 실행, 바인딩된 이벤트 싹다 Enable
        EnableAllActions();
    }

    protected override void OnDisable()
    {
        // 실행, 바인딩된 이벤트 싹다 Disable
        DisableAllActions();
    }

    public void Move()
    {
        Vector3 moveDirLocal = new Vector3(Owner.Dir.x, 0, Owner.Dir.y);

        // 애니메이션 이동 속도 조절
        SqrMoveManitude += moveDirLocal.sqrMagnitude * MOVESPEEDTEMP * Time.deltaTime;
        SqrMoveManitude = Mathf.Clamp(SqrMoveManitude, 0, 1);

        Owner.SetPositionByLocalDirection(moveDirLocal, MOVESPEEDTEMP);
    }

    public void Idle()
    {
        // 멈추면 
      
    }
}
