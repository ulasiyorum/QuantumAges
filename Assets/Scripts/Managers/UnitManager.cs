using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject unitMarker;
    private NavMeshAgent navMeshAgent;

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

    public void Moveto(Vector3 end)
    {
        navMeshAgent.SetDestination(end);   
    }

}
