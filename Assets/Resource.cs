using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float[] resources;
    public float amount = 100f;
    public int resourceType;
    public GameObject[] collection_points;

    private bool[] collection_points_usage;

    private void Start()
    {
        collection_points_usage = new bool[collection_points.Length];
        for (int i = 0; i < collection_points.Length; i++)
        {
            collection_points_usage[i] = false;
        }
    }

    private void Update()
    {
        
    }
    public bool collectResource(Soldier s)
    {
        for (int i = 0; i < collection_points_usage.Length; i++)
        {
            if (!collection_points_usage[i])
            {
                s.assigned_point = collection_points[i];    
                collection_points_usage[i] = true;  
                return true;
            }
        }
        return false;
    }
    public void AddCollectedResource(float amount)
    {
        // Toplanan kaynak miktarýný kaynak dizisine ekleyebilir veya baþka bir iþlem yapabilirsiniz.
        // Bu örnekte kaynaklarý sýfýrdan baþlatýyorum, sizin senaryonuza göre uygun iþlemleri yapmalýsýnýz.
        resources[0] += amount;
    }
}






