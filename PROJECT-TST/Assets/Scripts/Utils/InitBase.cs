using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitBase : MonoBehaviour
{
    protected bool _isInit = false;
    
    public virtual bool Init()
    {
        if (_isInit)
            return false;

        _isInit = true;
        return true;
    }

    void Awake()
    {
        Init();   
    }
}
