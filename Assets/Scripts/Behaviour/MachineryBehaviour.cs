using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consts;
using UnityEngine;
using UnityEngine.AI;

public class MachineryBehaviour : MonoBehaviour
{
    public bool isSelected = false;
    private Camera _camera;
    private bool moving = false;
    private bool attacking = false;
    private Animator animator;
    private NavMeshAgent agent;
    private CrystalBehaviour crystalToAttack;

    public static MachineryBehaviour greenMachine = GameObject.Find("GreenMachinery").GetComponent<MachineryBehaviour>();
    public static MachineryBehaviour redMachine = GameObject.Find("RedMachinery").GetComponent<MachineryBehaviour>();

    public UnitTeam unitTeam;
    [SerializeField] GameObject unitMarker;
    // Start is called before the first frame update
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    Select();
                }
            }
        }
        
        else if(isSelected && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                crystalToAttack = hit.transform.GetComponent<CrystalBehaviour>();
                SetDestination(hit.point);
                SetMoving(true);
            }
        }

        if (crystalToAttack is not null)
        {
            if (Vector3.Distance(transform.position, crystalToAttack.transform.position) < 2f)
            {
                SetDestination(transform.position);
                SetMoving(false);
                SetAttacking(true);
                crystalToAttack.StartBreaking(this);
            }
        }
    }

    public void EndBreaking()
    {
        SetAttacking(false);
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
