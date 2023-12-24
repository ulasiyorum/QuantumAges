using System.Collections.Generic;
using System.Threading.Tasks;
using Consts;
using Photon.Pun;
using UnityEngine;

public abstract class SoldierAnimator : MonoBehaviourPun
{
    private Animator animator;
    
    private bool hasDied = false;
    

    public static List<SoldierAnimator> soldiers = new List<SoldierAnimator>();
    private static readonly int AnimEnd = Animator.StringToHash("animEnd");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        soldiers.Add(this);
    }
    

    public abstract void Animate();

    protected virtual void OnMove()
    {
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(1,1);
        
        animator.SetBool("animEnd", false);
        animator.SetBool("move", true);
    }
    protected virtual void OnAttack() 
    {
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(2,1);
        animator.SetBool("animEnd", false);
        animator.SetBool("attack", true);
    }
    protected virtual void OnDie()
    {
        if (hasDied) return;
        animator.SetLayerWeight(3,1);
        animator.SetBool("die", true);
        hasDied = true;
    }
    protected virtual void OnIdle()
    {
        animator.SetLayerWeight(1,0);
        animator.SetLayerWeight(2,0);
        animator.SetLayerWeight(3,0);
        animator.SetLayerWeight(0,1);
        animator.SetBool("move", false);
        animator.SetBool("attack", false);
        animator.SetBool("animEnd", true);
    }
    
    protected void SetAnimationTrigger(bool value)
    {
        animator.SetBool("move", value);
        animator.SetBool("attack", value);
        animator.SetBool("animEnd", !value);
        
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
}
