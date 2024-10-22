using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseObject : InitBase
{
    public Collider Collider3D { get; private set; }
    public Transform Transform3D { get; private set; }
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
        Transform3D = GetComponent<Transform>();

        return true;
    }

    public virtual void SetInfo(string name)
    {
        // 데이터 자료 들고 올 예정 흠.. 굳이 csv? 해야할까......?????? 여쭤보자


    }

    public virtual void SetAnimator()
    {

    }

    public virtual void Clear()
    {

    }

    public virtual void SetPosition(Vector3 dir, float moveSpeed)
    {
        Vector3 localDir = transform.TransformDirection(dir);

        transform.position += moveSpeed * localDir.normalized * Time.deltaTime;
    }
}
