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
    
    public float _health;
    public float _damage;
    public float _range;

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
            
            if(!isMoving)
                OnIdle();
            
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

    public void AttackTo(UnitManager target)
    {
        var targetPosition = target.transform.position;
        var distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > _range)
        {
            MoveTo(targetPosition - transform.forward * _range);
            return;
        }
        
        isAttacking = true;
        OnAttack();
        StartCoroutine(target.TakeDamage(_damage, GetCurrentAnimationLength()));
    }
    
    public IEnumerator TakeDamage(float damage, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _health -= damage;
        if (_health <= 0)
        {
            isDead = true;
            yield break;
        }
        
        yield return TakeDamage(damage, seconds);
    }
}