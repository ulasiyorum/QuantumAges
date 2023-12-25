using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consts;
using Photon.Pun;
using UnityEngine;

public abstract class SoldierAnimator : MonoBehaviourPun
{
    protected bool animating;
    private Animator animator;
    protected bool hasDied = false;
    protected bool idle = false;

    public static List<SoldierAnimator> soldiers = new List<SoldierAnimator>();
    private static readonly int AnimEnd = Animator.StringToHash("animEnd");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        soldiers.Add(this);
    }
    

    public abstract void Animate();

    [PunRPC]
    protected virtual void OnMove()
    {
        animating = true;
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(1,1);
        
        animator.SetBool("animEnd", false);
        animator.SetBool("move", true);
        StartCoroutine(WaitForAnimationEnd());
    }
    
    [PunRPC]
    protected virtual void OnAttack()
    {
        animating = true;
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(2,1);
        animator.SetBool("animEnd", false);
        animator.SetBool("attack", true);
        StartCoroutine(WaitForAnimationEnd());
        
    }
    
    [PunRPC]
    protected virtual void OnDie()
    {
        if (hasDied) return;
        animator.SetLayerWeight(3,1);
        animator.SetBool("die", true);
        hasDied = true;
    }
    
    [PunRPC]
    protected virtual void OnIdle()
    {
        idle = true;
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(0,1);
        animator.SetBool("move", false);
        animator.SetBool("attack", false);
        animator.SetBool("animEnd", true);

    }
    protected float GetCurrentAnimationLength(AnimConsts animLayer)
    {
        var clipInfo = animator.GetCurrentAnimatorClipInfo((int)animLayer);

        return clipInfo[0].clip.length;
    }
    
    protected float GetNextAnimationLength(AnimConsts animLayer)
    {
        var clipInfo = animator.GetNextAnimatorClipInfo((int)animLayer);

        return clipInfo[0].clip.length;
    }

    protected IEnumerator WaitForAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        animating = false;
    }
}
