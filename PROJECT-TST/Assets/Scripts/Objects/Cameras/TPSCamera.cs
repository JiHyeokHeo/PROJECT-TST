using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTransposer;

public class TPSCamera : BaseCamera
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo()
    {
        CinemachineFramingTransposer hardLockToTarget = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (hardLockToTarget == null)
        {
            hardLockToTarget = _virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    Vector2 turn = Vector2.zero;
    public void LateUpdate()
    {
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }

    public override void SetFollowTarget(GameObject go)
    {
        base.SetFollowTarget(go);
    }
}
