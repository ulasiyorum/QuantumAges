using System;
using System.Collections;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Managers.Abstract;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitManager : SoldierAnimator, IDamagable
{
    private readonly Guid id = Guid.NewGuid();
    public SpriteRenderer healthBar;
    public GameObject unitMarker;
    private NavMeshAgent agent;
    public SoldierEnum soldierType;
    public UnitTeam unitTeam;
    private IDamagable currentTarget;
    private float maxHealth;
    public float _health;
    public float _damage;
    public float _range;

    protected override void Awake()
    {
        maxHealth = _health;
        agent = GetComponent<NavMeshAgent>();
        unitMarker.SetActive(false);
        RTSUnitManager.Instance.UnitList.Add(this);
        base.Awake();
    }

    private void Update()
    {
        _health = Mathf.Clamp(_health, 0f, maxHealth);

        float scaleX = _health / maxHealth;

        healthBar.transform.localScale = new Vector3(scaleX, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
    

    public override void Animate()
    {
        if (_health <= 0)
            OnDie();
        
        if (currentTarget == null
            && agent.pathStatus.Equals(NavMeshPathStatus.PathComplete))
            OnIdle();

        if (currentTarget != null && agent.hasPath)
            OnMove();
        
        if (currentTarget != null && !agent.hasPath)
            OnAttack();
        
        if(currentTarget == null && agent.hasPath)
            OnMove();
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
        OnMove();
    }
    public void SetTarget(IDamagable target, UnitTeam targetTeam)
    {
        currentTarget = target;

        if (target == null) return;
     
        if (targetTeam == unitTeam)
        {
            return;
        }

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
        OnAttack();

        length ??= 0.75f;
        target.TakeDamage(_damage);
        yield return new WaitForSeconds(length.Value);
        transform.rotation = Quaternion.LookRotation(targetPosition , transform.position);
        
        agent.StopAgent();
        StartCoroutine(AttackTo(target, length));
    }
    
    // [PunRPC]
    // public void Set 
    // {
    //     
    // }
    
    public IEnumerator AttackTo(MachineryBehaviour target, float? length = null)
    {
        if (currentTarget?.GetId() != target.GetId())
            yield break;
        
        var targetPosition = target.transform.position;
        var distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > _range)
        {
            MoveTo(targetPosition);
            yield return new WaitUntil(() => agent.remainingDistance <= _range);
        }
        OnAttack();

        length ??= 0.75f;
        yield return new WaitForSeconds(length.Value);
        agent.StopAgent();
        transform.LookAt(target.transform);
        target.TakeDamage(_damage);

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
            OnDie();
        }
    }
    
    
    [PunRPC]
    public void SetUnitTeam(int team)
    {
        unitTeam = (UnitTeam) team;
    }
}