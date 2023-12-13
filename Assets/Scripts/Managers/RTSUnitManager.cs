using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RTSUnitManager : MonoBehaviour
{
    private List<UnitManager> selectedUnitList;
    public List<UnitManager> UnitList { private set; get; }


    private void Awake()
    {
        selectedUnitList = new List<UnitManager>();
    }

    public void ClickSelectUnit(UnitManager newUnit)
    {
        DeselectAll();
        SelectUnit(newUnit);
    }

    public void ShiftClickSelectUnit(UnitManager newUnit)
    {
        if (selectedUnitList.Contains(newUnit))
            DeselectUnit(newUnit);
        else
            SelectUnit(newUnit);
    }

    public void DragSelectUnit(UnitManager newUnit)
    {
        if (!selectedUnitList.Contains(newUnit)) SelectUnit(newUnit);
    }

    public void MoveSelectedUnits(Vector3 end)
    {
        for (var i = 0; i < selectedUnitList.Count; i++) selectedUnitList[i].MoveTo(end);
    }

    public void DeselectAll()
    {
        for (var i = 0; i < selectedUnitList.Count; i++) selectedUnitList[i].DeselectUnit();
        selectedUnitList.Clear();
    }

    private void SelectUnit(UnitManager newUnit)
    {
        newUnit.SelectUnit();
        selectedUnitList.Add(newUnit);
    }

    private void DeselectUnit(UnitManager newUnit)
    {
        newUnit.DeselectUnit();
        selectedUnitList.Remove(newUnit);
    }
}