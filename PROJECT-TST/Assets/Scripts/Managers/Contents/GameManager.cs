using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject _vcamera;
    GameObject _player;
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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetCameraTarget(GameObject target)
    {
        if (VirtualCamera == null)
            return false;

        VirtualCamera.GetComponent<TCamera>().SetFollowTarget(target);

        return true;
    }
}
