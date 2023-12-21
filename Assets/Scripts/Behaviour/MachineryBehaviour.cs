using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Helpers;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.AI;
using PlayerManager = Managers.Abstract.PlayerManager;

public class MachineryBehaviour : MonoBehaviour
{
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
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
        animator = GetComponent<Animator>();

        if (unitTeam == UnitTeam.Green)
        {
            greenMachine = this;
            gameObject.AssignOwner(MultiplayerHelper.MasterPlayer);
        }
        else
        {
            redMachine = this;
            gameObject.AssignOwner(MultiplayerHelper.NonMasterPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.IsMine())
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
                }
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                crystalToAttack = hit.transform.GetComponent<Resource>();
                crystalToAttack.CollectResource(this, hit.point);
            }
        }

        if (crystalToAttack is not null && !attacking)
        {
            if (Vector3.Distance(transform.position, crystalToAttack.transform.position) < 2f)
            {
                agent.StopAgent();
                SetMoving(false);
                SetAttacking(true);
                StartCoroutine(StartCollecting());
            }
        }
    }

    public void CollectResource(Resource resource, Vector3 pointClicked)
    {
        SetDestination(pointClicked);
        SetMoving(true);
        crystalToAttack = resource;
    }

    private IEnumerator StartCollecting()
    {
        if(crystalToAttack is null) yield break;
        
        int collectionTime = (int) crystalToAttack.resourceType;

        yield return new WaitForSeconds(collectionTime);
        
        if(crystalToAttack is null) yield break;

        int colIndex = crystalToAttack.GetCollectionIndex();
        
        if(colIndex == -1) yield break;
        EndBreaking(colIndex);
    }

    public void EndBreaking(int colIndex)
    {
        crystalToAttack.EndBreaking(colIndex);

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
}
