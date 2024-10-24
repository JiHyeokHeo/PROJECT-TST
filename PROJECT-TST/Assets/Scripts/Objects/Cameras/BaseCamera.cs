using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCamera : InitBase
{
    protected GameObject _followTarget;
    protected GameObject _lookAtTarget;

    protected CinemachineVirtualCamera _virtualCamera;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        return true;
    }

    public virtual void SetInfo(GameObject followTarget, GameObject lookatTarget) { }

    public void SetTPSTarget(GameObject follow, GameObject lookat)
    {
        SetFollowTarget(follow);
        SetLookAtTarget(lookat);
    }

    public virtual void SetFollowTarget(GameObject go)
    {
        _virtualCamera.Follow = go.transform;
    }

    public virtual void SetLookAtTarget(GameObject go) 
    {
        _virtualCamera.LookAt = go.transform;
    }
}
