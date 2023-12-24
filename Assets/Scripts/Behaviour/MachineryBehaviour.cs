using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Helpers;
using Managers.Abstract;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.AI;
using PlayerManager = Managers.Abstract.PlayerManager;

public class MachineryBehaviour : MonoBehaviourPun, IDamagable
{
    private readonly Guid id = Guid.NewGuid();
    public SpriteRenderer healthBar;

    private float health = 100;
    private Vector3 spawnPos;
    public bool isSelected = false;
    private Camera _camera;
    private bool moving = false;
    private bool attacking = false;
    private Animator animator;
    private NavMeshAgent agent;
    private Resource crystalToAttack;
    public static MachineryBehaviour greenMachine;
    public static MachineryBehaviour redMachine;

    
    public UnitTeam unitTeam;
    [SerializeField] GameObject unitMarker;
    // Start is called before the first frame update
    
    void Awake()
    {
        spawnPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
        animator = GetComponent<Animator>();

        if (unitTeam == UnitTeam.Green)
        {
            greenMachine = this;
            //gameObject.AssignOwner(MultiplayerHelper.MasterPlayer);
        }
        else
        {
            redMachine = this;
            //gameObject.AssignOwner(MultiplayerHelper.NonMasterPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0f, 100f);

        float scaleX = health / 100f;

        healthBar.transform.localScale = new Vector3(scaleX, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        
        if (unitTeam == UnitTeam.Green && !MultiplayerHelper.MasterPlayer.IsLocal)
            return;
        
        if (unitTeam == UnitTeam.Red && !(MultiplayerHelper.NonMasterPlayer?.IsLocal ?? false))
            return;
        
        if (Input.GetMouseButton(0))
        { 
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    Select();
                }
                else
                {
                    DeSelect();
                }
            }
        }
        
        else if(isSelected && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                SetDestination(hit.point);
                SetMoving(true);
                if (crystalToAttack is not null)
                {
                    crystalToAttack.AbortCollecting();
                    crystalToAttack = null;
                    StopCoroutine(nameof(StartCollecting));
                    SetAttacking(false);
                }
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                crystalToAttack = hit.transform.GetComponent<Resource>();
                if (crystalToAttack is not null)
                {
                    crystalToAttack.CollectResource(this, hit.point);
                    return;
                }
            }
        }

        if (crystalToAttack is not null && !attacking)
        {
            if (agent.remainingDistance < 5f)
            {
                agent.StopAgent();
                SetMoving(false);
                SetAttacking(true);
                StartCoroutine(StartCollecting());
            }
        }
    }

    public void CollectResource(Resource resource, Vector3 pos)
    {
        SetDestination(pos);
        SetMoving(true);
        crystalToAttack = resource;
    }

    private IEnumerator StartCollecting()
    {
        if(crystalToAttack is null) yield break;

        int collectionTime = (int)crystalToAttack.resourceType;
        int colIndex = crystalToAttack.GetCollectionIndex();
        transform.rotation = crystalToAttack.GetTargetRotation(colIndex, transform.position);
        yield return new WaitForSeconds(collectionTime);
        
        if(crystalToAttack is null) yield break;
        
        if(colIndex == -1) yield break;
        EndBreaking(colIndex);
    }

    public void EndBreaking(int colIndex)
    {
        crystalToAttack.photonView.RPC("EndBreaking", RpcTarget.All , colIndex);

        if (unitTeam == UnitTeam.Green)
        {
            if(crystalToAttack.resourceType == ResourceType.BlueCrystal)
                PlayerManager.green_manager.blue_crystal_balance++;
            else
                PlayerManager.green_manager.green_crystal_balance++;
        }
        else
        {
            if(crystalToAttack.resourceType == ResourceType.BlueCrystal)
                PlayerManager.red_manager.blue_crystal_balance++;
            else
                PlayerManager.red_manager.green_crystal_balance++;
        }
        
        SetAttacking(false);
        crystalToAttack = null;
    }
    
    private void SetMoving(bool value)
    {
        moving = value;
        animator.SetBool("moving", moving);
    }

    private void SetAttacking(bool value)
    {
        attacking = value;
        animator.SetBool("attacking", attacking);
    }

    private void Select()
    {
        isSelected = true;
        unitMarker.SetActive(true);
        var removeItems = RTSUnitManager.Instance.UnitList.Where(x => x.unitTeam == unitTeam).ToList();
        
        foreach (var item in removeItems)
        {
            item.DeselectUnit();
        }
    }
    
    public void DeSelect()
    {
        isSelected = false;
        unitMarker.SetActive(false);
    }
    
    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public Guid GetId()
    {
        return id;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("I AM DEAD");
        transform.position = spawnPos;
        health = 100;
    }
}
