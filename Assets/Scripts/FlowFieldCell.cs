using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldCell 
{
    //private Vector3 _worldPos; //able to backtrack from gridIdx
    private Vector2Int _gridIndex; //able to backtrack from worldPos
    private byte _cost = 1;
    private ushort _bestCost = ushort.MaxValue;

    public FlowFieldCell(Vector2Int gridIndex)
    {
        _gridIndex = gridIndex;
        _cost = 1;
        _bestCost = ushort.MaxValue;
    }

    public byte Cost
    {
        set { _cost = value; }
        get { return _cost; }
    }
    public ushort BestCost
    {
        set { _bestCost = value; }
        get { return _bestCost; }
    }
    public Vector2Int GridIndex
    {
        get { return _gridIndex; }
    }
}