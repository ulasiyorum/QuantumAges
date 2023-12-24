using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Helpers;
using Photon.Pun;
using UnityEngine;

public class Resource : MonoBehaviourPun
{
    public Guid id = Guid.NewGuid();
    public ResourceType resourceType;
    public GameObject[] collection_points;
    private bool isRespawning = false;
    private bool[] collection_points_usage;
    private bool[] collected_points;

    private void Start()
    {
        collection_points_usage = new bool[collection_points.Length];
        collected_points = new bool[collection_points.Length];
        for (int i = 0; i < collection_points.Length; i++)
        {
            collection_points_usage[i] = false;
            collected_points[i] = false;
        }
    }

    private void Update()
    {
        
    }
    public bool CollectResource(MachineryBehaviour s, Vector3 pointClicked)
    {
        for (int i = 0; i < collection_points_usage.Length; i++)
        {
            collection_points_usage[i] = false;
        }

        float minDistance = float.MaxValue;
        int minIndex = 0;
        
        for (int i = 0; i < collection_points.Length; i++)
        {
            float distance = Vector3.Distance(collection_points[i].transform.position, pointClicked);
            if (distance < minDistance && !collected_points[i])
            {
                minDistance = distance;
                minIndex = i;
            }
        }
        
        if (collection_points_usage[minIndex])
        {
            return false;
        }
        else
        {
            collection_points_usage[minIndex] = true;
            s.CollectResource(this, collection_points[minIndex].transform.position);
            return true;
        }
        
        return false;
    }

    public Quaternion GetTargetRotation(int index, Vector3 point)
    {
        return Quaternion.LookRotation(collection_points[index].transform.position - point);
    }
    
    [PunRPC]
    public void EndBreaking(int collectionIndex)
    {
        for (int i = 0; i < collection_points_usage.Length; i++)
        {
            collection_points_usage[i] = false;
        }
        
        collected_points[collectionIndex] = true;
        
        collection_points[collectionIndex].SetActive(false);
        
        if(!isRespawning)
            StartCoroutine(Respawn());
    }
    
    public void AbortCollecting()
    {
        for (int i = 0; i < collection_points_usage.Length; i++)
        {
            collection_points_usage[i] = false;
        }
    }
    
    public int GetCollectionIndex()
    {
        for (int i = 0; i < collection_points_usage.Length; i++)
        {
            if (collection_points_usage[i])
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;
        
        if (!collected_points.Any(x => x))
        {
            isRespawning = false;
            yield break;
        }

        int respawnTime = (int)resourceType * 3;
        yield return new WaitForSeconds(respawnTime);
        
        for (int i = 0; i < collection_points.Length; i++)
        {
            collection_points[i].SetActive(true);
            collected_points[i] = false;
        }
        
        isRespawning = false;
    }
}






