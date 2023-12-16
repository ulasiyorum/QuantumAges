using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consts;
using UnityEngine;
using UnityEngine.AI;
public class UnitManager : SoldierAnimator
{
    public GameObject unitMarker;
    private NavMeshAgent agent;
    public SoldierEnum soldierType;
    public UnitTeam unitTeam;
    private bool isMoving;
    private bool isAttacking;
    private bool isDead;
    private bool isIdle;

    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unitMarker.SetActive(false);
        RTSUnitManager.Instance.UnitList.Add(this);
        base.Awake();
    }

    public override Task Animate()
    {
        if (isDead)
        {
            OnDie();
            isDead = false;
            return Task.CompletedTask;
        }
        if (isMoving)
        {
            OnMove();
            isMoving = agent.remainingDistance > 0.1f;
            return Task.CompletedTask;
        }
        if (isAttacking)
        {
            OnAttack();
            isAttacking = false;
            return Task.CompletedTask;
        }
        if (isIdle)
        {
            OnIdle();
            isIdle = false;
            return Task.CompletedTask;
        }
        
        SetAnimationTrigger(false);
        return Task.CompletedTask;
    }

    public void SelectUnit()
    {
        unitMarker.SetActive(true);
    }

    public void DeselectUnit()
    {
        unitMarker.SetActive(false);
    }

    public void MoveTo(Vector3 end)
    {
        agent.SetDestination(end);
        isMoving = true;
    }
}