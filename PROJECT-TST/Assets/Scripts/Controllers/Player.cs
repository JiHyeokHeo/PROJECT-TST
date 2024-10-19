using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Player : Creature
{
    [SerializeField]
    float speed = 5.0f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;


        return true;
    }

    public void Update()
    {

        // AddForce¿¡´Ù°£ TimedeltaTime xx Always from FixedUpdate 
        if (Input.GetKey(KeyCode.A))
        {
            RigidBody.AddForce(-speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RigidBody.AddForce(speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            RigidBody.AddForce(0, 0, speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            RigidBody.AddForce(0, -speed, -speed);
        }

    }


    protected override void AnimPlayIdle()
    {
        CAnimator.SetFloat("idle_run_ratio", 1.0f);
        CAnimator.Play("Idle_Run");
    }
}
