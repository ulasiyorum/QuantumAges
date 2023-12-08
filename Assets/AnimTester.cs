using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimTester : SoldierAnimator
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override Task Animate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            OnAttack();
            return Task.CompletedTask;
        }

        if (Input.GetKey(KeyCode.D))
        {
            OnMove();
            return Task.CompletedTask;
        }

        if (Input.GetKey(KeyCode.S))
        {
            OnIdle();
            return Task.CompletedTask;
        }

        if (Input.GetKey(KeyCode.W))
        {
            OnDie();
            return Task.CompletedTask;
        }


        SetAnimationTrigger(false);
        return Task.CompletedTask;
    }
}
