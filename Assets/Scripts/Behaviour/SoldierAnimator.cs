using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoldierAnimator : MonoBehaviour
{
    public Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Animate();
    }

    public abstract void Animate();

    protected virtual void OnMove()
    {
        animator.Play("move");
    }
    protected virtual void OnAttack() 
    {
        animator.Play("attack");
    }
    protected virtual void OnDie()
    {
        animator.Play("die");
    }
    protected virtual void OnIdle()
    {
        animator.Play("idle");
    }
}
