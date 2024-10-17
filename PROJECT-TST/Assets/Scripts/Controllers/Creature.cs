using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define.Define;

public class Creature : BaseObject
{
    // FSM AI 시간 조절
    [SerializeField]
    WaitForSeconds seconds = new WaitForSeconds(1);

    Animator _animator;
    ECharactorState _creatureState;
    Coroutine _fsmState;

    public Animator Animator
    {
        get { return _animator; }
        set { _animator = value; }
    }

    public virtual ECharactorState CreatureState
    {
        get { return _creatureState; }
        set 
        {
            _creatureState = value;
            if (_animator != null)
                PlayAnimation();
        }
    }

    public Coroutine FsmState
    {
        get { return _fsmState; }
        private set { }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _animator = GetComponent<Animator>();
        _fsmState = StartCoroutine(ICoroutineAI());
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual IEnumerator ICoroutineAI()
    {
        while (true)
        {
            switch (CreatureState)
            {
                case ECharactorState.Idle:
                    Idle();
                    break;
                case ECharactorState.Move:
                    Move();
                    break;
                case ECharactorState.Attack:
                    Attack();
                    break;
                case ECharactorState.Skill:
                    UseSkill();
                    break;
                case ECharactorState.Damaged:
                    OnDamaged();
                    break;
                case ECharactorState.Die:
                    Dead();
                    break;
            }

            // 시간 조절
            yield return seconds;
        }
    }

    public virtual void Idle()
    {

    }

    public virtual void Move()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void UseSkill()
    {

    }

    public virtual void OnDamaged()
    {

    }

    public virtual void Dead()
    {

    }

    public virtual void PlayAnimation()
    {
        // 애니메이션 관련 넣으면 될듯?

        switch (CreatureState) 
        {
            case ECharactorState.Idle:
                break;
            case ECharactorState.Move:
                break;
            case ECharactorState.Attack:
                break;
            case ECharactorState.Skill:
                break;
            case ECharactorState.Damaged:
                break;
            case ECharactorState.Die:
                break;
        }
    }

    public override void Clear() 
    {
        base.Clear();
    }
}
