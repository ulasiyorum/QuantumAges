using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SoldierAnimator : MonoBehaviour
{
    private Animator animator;
    
    private bool animPlaying = false;

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

    void Awake()
    {
        animator = GetComponent<Animator>();
        soldiers.Add(this);
    }
    

    public abstract Task Animate();

    protected virtual void OnMove()
    {
        AnimPlaying = true;
        animator.Play("move");
    }
    protected virtual void OnAttack() 
    {
        AnimPlaying = true;
        animator.Play("attack");
    }
    protected virtual void OnDie()
    {
        AnimPlaying = true;
        animator.Play("die");
    }
    protected virtual void OnIdle()
    {
        AnimPlaying = true;
        animator.Play("idle");
    }
    
    protected void SetAnimationTrigger(bool value)
    {
        AnimPlaying = value;
    }
}
