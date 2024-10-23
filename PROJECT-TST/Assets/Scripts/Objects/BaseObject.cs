using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseObject : InitBase
{
    // ScriptAbleObject 데이터로 이름 관리
    public Collider Collider3D { get; private set; }
    public Transform Transform3D { get; private set; }
    Vector3 _pos = Vector3.zero;
    Vector3 _dir = Vector3.zero;
    Vector3 _magnitude= Vector3.zero;

    public Vector3 Pos
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public Vector3 Dir
    {
        get { return _dir; }
        set { _dir = value; }
    }

    public Vector3 Magnitude
    {
        get { return _magnitude; }
        set { _magnitude = value; }
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

    public virtual void SetPositionByLocalDirection(Vector3 dir, float MOVESPEEDTEMP)
    {
        Vector3 localDir = transform.TransformDirection(dir);

        transform.position += MOVESPEEDTEMP * localDir.normalized * Time.deltaTime;
    }
}
