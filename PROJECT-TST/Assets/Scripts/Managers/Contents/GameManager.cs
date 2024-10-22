using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{ 
    GameObject _vcamera;
    GameObject _player;
    //private List<GameObject> _virtualCameras = new List<GameObject>();

    public GameObject VirtualCamera
    {
        get { return _vcamera; }
        set { _vcamera = value; }
    }

    public GameObject Player
    { 
        get { return _player; } 
        set {  _player = value; }
    }

    public T SetCameraTarget<T>(GameObject target) where T : BaseCamera
    {
        if (VirtualCamera.IsValid() == false)
            return null;

        GameObject followShoulderTarget = Util.FindChild(target, name: "FollowTarget", true);
        if (followShoulderTarget == null)
        {
            GameObject go = new GameObject();
            go.name = "FollowTarget";
            go.transform.parent = target.transform;
        }

        // 추후 1인칭 추가 할때도 문제 없게 하기 위해
        VirtualCamera.GetComponent<T>().SetFollowTarget(followShoulderTarget);
        VirtualCamera.GetComponent<T>().SetInfo();
        return null;
    }
}
