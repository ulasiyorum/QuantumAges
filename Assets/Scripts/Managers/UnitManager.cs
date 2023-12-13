using System.Collections;
using System.Collections.Generic;
using Consts;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public GameObject unitMarker;
    private NavMeshAgent navMeshAgent;
    public SoldierEnum soldierType;
    public UnitTeam unitTeam;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        navMeshAgent.SetDestination(end);
    }
}