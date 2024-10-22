using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCamera : InitBase
{
    protected CinemachineVirtualCamera _virtualCamera;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        return true;
    }

    public virtual void SetInfo() { }

    public virtual void SetFollowTarget(GameObject go)
    {
        _virtualCamera.Follow = go.transform;
    }
}
