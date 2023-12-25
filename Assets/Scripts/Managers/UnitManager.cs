using System;
using System.Collections;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Managers;
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
    
    public SpriteRenderer healthBarBG;
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
        if (animating)
            return;
        
        if (agent is null)
            return;

        if (_health <= 0 && !hasDied)
        {
            hasDied = true;
            photonView.RPC("OnDie", RpcTarget.All);
            return;
        }


        // if (currentTarget == null
        //     && agent.pathStatus.Equals(NavMeshPathStatus.PathComplete))
        // {
        //     photonView.RPC("OnIdle", RpcTarget.All);
        //     return;
        // }

        if (currentTarget != null && agent.hasPath)
        {
            idle = false;
            photonView.RPC("OnMove", RpcTarget.All);
            return;
        }
        
        if (currentTarget != null && !agent.hasPath)
        {
            idle = false;
            photonView.RPC("OnAttack", RpcTarget.All);
            return;
        }

        if (currentTarget == null && agent.hasPath)
        {
            idle = false;
            photonView.RPC("OnMove", RpcTarget.All);
            return;
        }
        
        if (currentTarget == null && !agent.hasPath && !idle)
        {
            idle = true;
            photonView.RPC("OnIdle", RpcTarget.All);
            return;
        }
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
    public void SetTarget(IDamagable target, UnitTeam targetTeam, Transform overrideTransform = null)
    {
        currentTarget = target;

        if (target == null) return;
     
        if (targetTeam == unitTeam)
        {
            currentTarget = null;
            return;
        }

        if(target.GetType() == typeof(MachineryBehaviour))
            StartCoroutine(AttackTo((MachineryBehaviour)target));
        
        else if(target.GetType() == typeof(UnitManager))
            StartCoroutine(AttackTo((UnitManager)target));
        
        else if(target.GetType() == typeof(PlayerManager))
            StartCoroutine(AttackTo((PlayerManager)target , overrideTransform));
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
        target.photonView.RPC("TakeDamage", RpcTarget.All, _damage);
        yield return new WaitForSeconds(length.Value);
        transform.rotation = Quaternion.LookRotation(targetPosition , transform.position);
        
        agent.StopAgent();
        StartCoroutine(AttackTo(target, length));
    }

    public IEnumerator AttackTo(PlayerManager target, Transform overrideTransform, float? length = null)
    {
        if(unitTeam == target.team)
            yield break;
        
        var targetPosition = target.transform.position;
        
        var distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > _range * 2.5f + 2.5f)
        {
            MoveTo(targetPosition);
            yield return new WaitUntil(() => agent.remainingDistance <= _range);
        }
        
        OnAttack();
        
        length ??= 0.75f;
        yield return new WaitForSeconds(length.Value);
        agent.StopAgent();
        transform.LookAt(overrideTransform);
        target.photonView.RPC("TakeDamage", RpcTarget.All, _damage);
        
        StartCoroutine(AttackTo(target, overrideTransform , length.Value));
    }

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
        target.photonView.RPC("TakeDamage", RpcTarget.All, _damage);

        StartCoroutine(AttackTo(target, length.Value));
    }

    public Guid GetId()
    {
        return id;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            RTSUnitManager.Instance.selectedUnitList.Remove(this);
            soldiers.Remove(this);
            photonView.RPC("SetCurrentTarget", RpcTarget.Others);
            OnDie();
            
            if(unitTeam == UnitTeam.Green)
                PlayerManager.red_manager.GetKill();
            else
                PlayerManager.green_manager.GetKill();
        }
    }
    
    
    [PunRPC]
    public void SetUnitTeam(int team)
    {
        unitTeam = (UnitTeam) team;
        
        var localTeam = PhotonNetwork.MasterClient.IsLocal ? UnitTeam.Green : UnitTeam.Red;

        if (localTeam != unitTeam)
        {
            healthBar.gameObject.SetActive(false);
            healthBarBG.gameObject.SetActive(false);
        }
    }
    
    [PunRPC]
    public void SetCurrentTarget()
    {
        currentTarget = null;
    }
}