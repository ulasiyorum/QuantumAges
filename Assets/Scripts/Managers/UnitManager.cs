using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Managers.Abstract;
using UnityEngine;
using UnityEngine.AI;
public class UnitManager : SoldierAnimator, IDamagable
{
    private Guid id = Guid.NewGuid();
    
    public GameObject unitMarker;
    private NavMeshAgent agent;
    public SoldierEnum soldierType;
    public UnitTeam unitTeam;
    private bool isMoving;
    private bool isAttacking;
    private bool isDead;
    private bool isIdle;
    private IDamagable currentTarget;
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
    public void SetTarget(IDamagable target)
    {
        currentTarget = target;

        if (target == null) return;
        
        if(target.GetType() == typeof(MachineryBehaviour))
            StartCoroutine(AttackTo((MachineryBehaviour)target));
        
        else if(target.GetType() == typeof(UnitManager))
            StartCoroutine(AttackTo((UnitManager)target));
    }
    public IEnumerator AttackTo(UnitManager target, float? length = null)
    {
        if(currentTarget.GetId() != target.GetId())
            yield break;
        
        var targetPosition = target.transform.position;
        var distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > _range)
        {
            MoveTo(targetPosition);
            yield break;
        }

        length ??= GetCurrentAnimationLength();
        target.TakeDamage(_damage);
        agent.StopAgent();
        isMoving = false;
        isAttacking = true;
        OnAttack();
        StartCoroutine(AttackTo(target, length));
    }
    
    public IEnumerator AttackTo(MachineryBehaviour target, float? length = null)
    {
        if (currentTarget.GetId() != target.GetId())
            yield break;
        
        var targetPosition = target.transform.position;
        var distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > _range)
        {
            MoveTo(targetPosition - transform.forward * _range);
            yield break;
        }
        length ??= GetCurrentAnimationLength();
        target.TakeDamage(_damage);
        isAttacking = true;
        OnAttack();
        StartCoroutine(AttackTo(target, length.Value));
    }

    public Guid GetId()
    {
        return id;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            isDead = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            return;
        
        agent.StopAgent();
        isMoving = false;
    }
}