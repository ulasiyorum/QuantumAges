using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public void ClickSelectUnit(UnitController newUnit)
    {
        DeselectAll();
        SelectUnit(newUnit);
    }
    public void ShiftClickSelectUnit(UnitController newUnit)
    {
     if(selectedUnitList.Contains(newUnit))
        {
            DeselectUnit(newUnit);
        }
        else
        {
            SelectUnit(newUnit);
        }
    }   

    public void DragSelectUnit(UnitController newUnit)
    {
        if(!selectedUnitList.Contains(newUnit))
        {
            SelectUnit(newUnit);
        }   
    }

    public void MoveSelectedUnits(Vector3 end)
    {
        for(int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].Moveto(end);
        }
    }

    public void DeselectAll()
    {
          for(int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].DeselectUnit();
        }
        selectedUnitList.Clear();   
    }

    private void SelectUnit(UnitController newUnit)
    {
      newUnit.SelectUnit();
        selectedUnitList.Add(newUnit);
    }
    
    private void DeselectUnit(UnitController newUnit)
    {
        newUnit.DeselectUnit();
        selectedUnitList.Remove(newUnit);
    }
}
