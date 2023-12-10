using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSUnitController : MonoBehaviour
{
    [SerializeField]
    private UnitSpawner unitSpawner;
    private List<UnitController> selectedUnitList;
    public List<UnitController> UnitList { private set; get; }


    private void Awake()
    {
        selectedUnitList = new List<UnitController>();  
        UnitList = unitSpawner.SpawnUnits();    
    }
}
