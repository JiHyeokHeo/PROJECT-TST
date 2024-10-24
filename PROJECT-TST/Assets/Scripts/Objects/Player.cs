using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Defines.Define;

public class Player : Creature
{
    PlayerController _controller;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _controller = this.GetOrAddComponent<PlayerController>();
        _controller.SetInfo();


        SetInfo("PlayerTEMP");
        return true;
    }

    public override void SetInfo(string name)
    {
        CreatureState = ECharactorState.Idle;
    }

    public override void Idle()
    {
        if (_controller == null)
            return;
        // 눌린 힘이 있다면 바로 무브 스테이드
        if (_controller.SqrInputMagnitude > 0)
        {
            CreatureState = ECharactorState.Move;
            return;
        }

        // 눌린 힘 X 그러면 자연스럽게 애니메이터 속도 조절
        if (_controller.SqrMoveManitude > 0)
        {
            _controller.SqrMoveManitude -= HARDCODING.MOVESPEEDTEMP * Time.deltaTime;
            Mathf.Clamp01(_controller.SqrMoveManitude);
            CAnimator.SetFloat("idle_run_ratio", _controller.SqrMoveManitude);
        }
    }

    public override void Move()
    {
        if (_controller == null)
            return;

        // 만약 움직임 인풋 힘이 0이면 Idle 상태로 전환
        if (_controller.SqrInputMagnitude <= 0)
            CreatureState = ECharactorState.Idle;

        // 컨트롤 움직임
        _controller.Move();

        // 그거에 관해 무브 애니메이터 블랜드
        CAnimator.SetFloat("idle_run_ratio", _controller.SqrMoveManitude);
    }

    protected override void AnimPlayIdle()
    {
        CAnimator.Play("Idle_Run");
    }

    protected override void AnimPlayMove()
    {
        CAnimator.Play("Idle_Run");
    }
}
