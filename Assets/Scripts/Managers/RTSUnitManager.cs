using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Unity.VisualScripting;
using UnityEngine;

public class RTSUnitManager : MonoBehaviour
{
    public static RTSUnitManager Instance { private set; get; }
    private List<UnitManager> selectedUnitList;
    public List<UnitManager> UnitList { private set; get; }


    private void Awake()
    {
        Instance = this;
        selectedUnitList = new List<UnitManager>();
        UnitList = new List<UnitManager>();
    }

    public void ClickSelectUnit(UnitManager newUnit)
    {
        DeselectAll(UnitTeam.Green);
        DeselectAll(UnitTeam.Red);
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

    public void MoveSelectedUnits(Vector3 end, UnitTeam team)
    {
        foreach (var t in selectedUnitList.Where(x => x.unitTeam == team))
            t.MoveTo(end);
    }

    public void DeselectAll(UnitTeam team)
    {
        foreach (var t in selectedUnitList.Where(x => x.unitTeam == team))
            t.DeselectUnit();

        selectedUnitList.RemoveAll(x => x.unitTeam == team);
    }

    private void SelectUnit(UnitManager newUnit)
    {
        if(newUnit.unitTeam == UnitTeam.Green)
            MachineryBehaviour.greenMachine.DeSelect();
        
        else if(newUnit.unitTeam == UnitTeam.Red)
            MachineryBehaviour.redMachine.DeSelect();
        
        newUnit.SelectUnit();
        selectedUnitList.Add(newUnit);
    }

    private void DeselectUnit(UnitManager newUnit)
    {
        newUnit.DeselectUnit();
        selectedUnitList.Remove(newUnit);
    }
    
    public void AttackTo(UnitManager target)
    {
        foreach (var unit in selectedUnitList)
        {
            unit.AttackTo(target);
        }
    }
}