using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Defines.Define;

public class Creature : BaseObject
{
    // FSM AI 시간 조절
    [SerializeField]
    WaitForSeconds seconds = new WaitForSeconds(1);

    Animator _animator;
    ECharactorState _creatureState;
    Coroutine _fsmState;
    Rigidbody _rigidBody;

    public Animator CAnimator
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

    public Rigidbody RigidBody
    {
        get { return _rigidBody; }
        set { _rigidBody = value; }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CAnimator = GetComponent<Animator>();
        FsmState = StartCoroutine(ICoroutineAI());
        RigidBody = GetComponent<Rigidbody>();

        CreatureState = ECharactorState.Idle;
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
                AnimPlayIdle();
                break;
            case ECharactorState.Move:
                AnimPlayMove();
                break;
            case ECharactorState.Attack:
                AnimPlayAttack();
                break;
            case ECharactorState.Skill:
                AnimPlaySkill();
                break;
            case ECharactorState.Damaged:
                AnimPlayDamaged();
                break;
            case ECharactorState.Die:
                AnimPlayDie();
                break;
        }
    }

    protected virtual void AnimPlayIdle()
    {

    }

    protected virtual void AnimPlayMove()
    {

    }

    protected virtual void AnimPlayAttack()
    {

    }

    protected virtual void AnimPlaySkill()
    {

    }

    protected virtual void AnimPlayDamaged()
    {

    }

    protected virtual void AnimPlayDie()
    {

    }

    public override void Clear() 
    {
        base.Clear();
    }
}
