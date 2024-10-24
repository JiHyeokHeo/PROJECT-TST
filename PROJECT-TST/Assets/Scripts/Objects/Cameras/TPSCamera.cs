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

    public override void SetInfo(GameObject followTarget, GameObject lookatTarget)
    {
        // 일단 TPS 전용 타겟 설정
        _followTarget = followTarget;
        _lookAtTarget = lookatTarget;
        SetTPSTarget(followTarget, lookatTarget);

        Cinemachine3rdPersonFollow thirdPersonFollow = _virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = _virtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        CinemachineHardLookAt hardLockToTarget = _virtualCamera.GetCinemachineComponent<CinemachineHardLookAt>();
        if (hardLockToTarget == null)
        {
            hardLockToTarget = _virtualCamera.AddCinemachineComponent<CinemachineHardLookAt>();
        }
    }
}
