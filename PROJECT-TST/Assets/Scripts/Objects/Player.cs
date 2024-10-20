using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player : Creature
{
    IController _controller;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _controller = this.GetOrAddComponent<PlayerController>();
        _controller.SetInfo();

        Managers.GameManager.SetCameraTarget(gameObject);
        return true;
    }

    void Update()
    {
        _controller.Move();
    }


    protected override void AnimPlayIdle()
    {
        CAnimator.SetFloat("idle_run_ratio", 1.0f);
        CAnimator.Play("Idle_Run");
    }
}
