using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCamera : InitBase
{
    CinemachineVirtualCamera _virtualCamera;
    // Start is called before the first frame update

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        return true;
    }

    public void SetFollowTarget(GameObject go)
    {
        _virtualCamera.Follow = go.transform;
    }
}
