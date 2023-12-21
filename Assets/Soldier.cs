using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
  private NavMeshAgent agent;

    public float health;
    public float attack;

    public float resource_capacity;
    public float resource_remaining;
    private float collectedResourceAmount;  

    public GameObject assignedPoint;
    public GameObject returnPoint;

    public bool can_collect;
    public bool collecting_resource;

    public GameObject assigned_point;
    private MainEvents mainEvents;

     void Start()
    {
        collectedResourceAmount = 0f;   
        agent = GetComponent<NavMeshAgent>();
       mainEvents = GameObject.Find("MainEvents").GetComponent<MainEvents>();  
    }

    private void Update()
    {
        if(!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    //agent reached destination
                    collecting_resource = true;
                }
            }
        }
        if (collecting_resource)
        {
            if(resource_remaining < resource_capacity)
            {
                resource_remaining += Time.deltaTime * 2f;

                // Toplanan miktarý güncelle
                collectedResourceAmount = resource_remaining;
            }
            else
            {
                collecting_resource = false;
                resource_remaining = resource_capacity;
                SendCollectedResource();
                Walk(returnPoint.transform.position);
            }   
        }
    }
     private void SendCollectedResource()
    {
        // Toplanan miktarý MainEvents scriptine iletmek için çaðrý yap
        if (mainEvents != null)
        {
            mainEvents.AddCollectedResource(collectedResourceAmount);
        }

        // Toplanan miktarý sýfýrla
        collectedResourceAmount = 0f;
    }

    public void Walk(Vector3 location)
    {
          agent.SetDestination(location);   
    }

    public void GoToResource(Resource resource)
    {
       if(resource.collectResource(this))
        {
            //kaynak toplamaya baslanabilir

            Walk(assignedPoint.transform.position);  
}
    }



}
