using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static Defines.Define;

public class Player : Creature
{
    IController _controller;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _controller = this.GetOrAddComponent<PlayerController>();
        _controller.SetInfo();

        // TEMP
        CreatureState = ECharactorState.Move;

        return true;
    }

    public override void Move()
    {
        _controller.Move();
    }

    protected override void AnimPlayIdle()
    {
        CAnimator.SetFloat("idle_run_ratio", 0.0f);
        CAnimator.Play("Idle_Run");
    }

    protected override void AnimPlayMove()
    {
        CAnimator.SetFloat("idle_run_ratio", 1.0f);
        CAnimator.Play("Idle_Run");
    }
}
