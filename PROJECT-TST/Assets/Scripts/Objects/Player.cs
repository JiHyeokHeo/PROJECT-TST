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
        // ���� ���� �ִٸ� �ٷ� ���� �����̵�
        if (_controller.SqrInputMagnitude > 0)
        {
            CreatureState = ECharactorState.Move;
            return;
        }

        // ���� �� X �׷��� �ڿ������� �ִϸ����� �ӵ� ����
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

        // ���� ������ ��ǲ ���� 0�̸� Idle ���·� ��ȯ
        if (_controller.SqrInputMagnitude <= 0)
            CreatureState = ECharactorState.Idle;

        // ��Ʈ�� ������
        _controller.Move();

        // �װſ� ���� ���� �ִϸ����� ����
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
