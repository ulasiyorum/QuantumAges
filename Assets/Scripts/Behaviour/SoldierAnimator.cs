using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SoldierAnimator : MonoBehaviour
{
    private Animator animator;
    
    private bool animPlaying = false;
    private bool hasDied = false;

    private bool AnimPlaying
    {
        get => animPlaying;
        set
        {
            if (animPlaying != value)
            {
                animator.SetBool(AnimEnd, !value);
            }
            animPlaying = value;
        }   
    }

    public static List<SoldierAnimator> soldiers = new List<SoldierAnimator>();
    private static readonly int AnimEnd = Animator.StringToHash("animEnd");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        soldiers.Add(this);
    }
    

    public abstract Task Animate();

    protected virtual void OnMove()
    {
        AnimPlaying = true;
        animator.SetBool("move", true);
    }
    protected virtual void OnAttack() 
    {
        AnimPlaying = true;
        animator.SetBool("attack", true);
    }
    protected virtual void OnDie()
    {
        if (hasDied) return;
        AnimPlaying = true;
        animator.SetBool("die", true);
        hasDied = true;
    }
    protected virtual void OnIdle()
    {
        AnimPlaying = true;
        animator.SetBool("animEnd", true);
    }
    
    protected void SetAnimationTrigger(bool value)
    {
        AnimPlaying = value;
        animator.SetBool("move", value);
        animator.SetBool("attack", value);
        animator.SetBool("animEnd", !value);
        
    }
    
    protected float GetCurrentAnimationLength()
    {
        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        return clipInfo[0].clip.length;
    }
}
