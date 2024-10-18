using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player : Creature
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        return true;
    }

    public void Update()
    {
    }


    protected override void AnimPlayIdle()
    {
        CAnimator.SetFloat("idle_run_ratio", 1.0f);
        CAnimator.Play("Idle_Run");
    }
}
