using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseObject : InitBase
{
    public Collider Collider3D { get; private set; }
    Vector3 _pos = Vector3.zero;

    public Vector3 Pos
    {
        get { return _pos; }
        set
        {
            _pos = value;
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider3D = this.GetOrAddComponent<Collider>();


        return true;
    }

    public virtual void SetInfo(string name)
    {
        // ������ �ڷ� ��� �� ���� ��.. ���� csv? �ؾ��ұ�......?????? ���庸��


    }

    public virtual void SetAnimator()
    {

    }

    public virtual void Clear()
    {

    }
}