using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldGrid
{
    private bool _isDiagonal = true;
    private int _rows = 20;
    private int _cols = 40;
    private float _cellSize = 10.0f;

    private FlowFieldCell[] _grid;
    private List<FlowFieldCell>[] _neighbours;

    public FlowFieldGrid(Vector2Int dimensions, float cellSize = 10.0f, bool isDiagonal = true)
    {
        _rows = dimensions.x;
        _cols = dimensions.y;
        _cellSize = cellSize;
        _isDiagonal = isDiagonal;

        CreateGrid();
        IdentifyAllNeighbours();
    }

    private void CreateGrid()
    {
        _grid = new FlowFieldCell[_rows * _cols];

        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _cols; ++j)
            {
                _grid[i + j * _rows] = new FlowFieldCell(new Vector2Int(i, j));
            }
        }
    }

    private void IdentifyAllNeighbours()
    {
        _neighbours = new List<FlowFieldCell>[_rows * _cols];

        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _cols; ++j)
            {
                int currentIdx = i + j * _rows;
                int size = 4; //4 directions
                if (_isDiagonal)
                    size = 8; //+ 4 diagonal ones
                _neighbours[currentIdx] = new List<FlowFieldCell>(size);

                //left side from current unit
                if (i - 1 >= 0)
                {
                    _neighbours[currentIdx].Add(_grid[i - 1 + j * _rows]);
                    //diagonals
                    if (_isDiagonal)
                    {
                        //left-top side
                        if (j + 1 < _cols)
                            _neighbours[currentIdx].Add(_grid[i - 1 + (j + 1) * _rows]);
                        //left-bottom side
                        if (j - 1 >= 0)
                            _neighbours[currentIdx].Add(_grid[i - 1 + (j - 1) * _rows]);
                    }
                }
                //right side from current unit
                if (i + 1 < _rows)
                {
                    _neighbours[currentIdx].Add(_grid[i + 1 + j * _rows]);
                    //diagonals
                    if (_isDiagonal)
                    {
                        //right-top side
                        if (j + 1 < _cols)
                            _neighbours[currentIdx].Add(_grid[i + 1 + (j + 1) * _rows]);
                        //right-bottom side
                        if (j - 1 >= 0)
                            _neighbours[currentIdx].Add(_grid[i + 1 + (j - 1) * _rows]);
                    }
                }

                //top side from current unit
                if (j + 1 < _cols)
                    _neighbours[currentIdx].Add(_grid[i + (j + 1) * _rows]);

                //bottom side from current unit
                if (j - 1 >= 0)
                    _neighbours[currentIdx].Add(_grid[i + (j - 1) * _rows]);
            }
        }
    }

    public int Rows
    {
        set { _rows = value; }
        get { return _rows; }
    }
    public int Colums
    {
        set { _cols = value; }
        get { return _cols; }
    }
    public bool IsDiagonal
    {
        set { _isDiagonal = value; }
        get { return _isDiagonal; }
    }
    public float CellSize
    {
        set { _cellSize = value; }
        get { return _cellSize; }
    }
    public FlowFieldCell[] Cells
    {
        get { return _grid; }
    }
    //returns X == amount of rows, Y == amount of cols and Z == unit size
    public Vector3 Dimensions
    {
        get { return new Vector3(_rows, _cols, _cellSize); }
    }

    public FlowFieldCell GetCellFromGridIndex(Vector2Int gridIndex)
    {
        if (gridIndex.x >= _rows || gridIndex.y >= _cols)
            return null;

        return _grid[gridIndex.x + gridIndex.y * _rows];
    }

    public FlowFieldCell GetCellFromWorldPos(Vector3 worldPos)
    {
        int xIdx = (int)(worldPos.x / _cellSize);
        int yIdx = (int)(worldPos.z / _cellSize);

        if (xIdx >= _rows || yIdx >= _cols)
            return null;

        return _grid[xIdx + yIdx * _rows];
    }

    public List<FlowFieldCell> GetNeighboursFromGridIndex(Vector2Int gridIndex)
    {
        if (gridIndex.x >= _rows || gridIndex.y >= _cols)
            return null;

        return _neighbours[gridIndex.x + gridIndex.y * _rows];
    }

    public List<FlowFieldCell> GetNeighboursFromWorldPos(Vector3 worldPos)
    {
        int xIdx = (int)(worldPos.x / _cellSize);
        int yIdx = (int)(worldPos.z / _cellSize);

        if (xIdx >= _rows || yIdx >= _cols)
            return null;

        return _neighbours[xIdx + yIdx * _rows];
    }

    private void GenerateIntegrationField(FlowFieldCell _destinationCell)
    {
        _destinationCell.Cost = 0;
        _destinationCell.BestCost = 0;

        Queue<FlowFieldCell> openList = new Queue<FlowFieldCell>();

        openList.Enqueue(_destinationCell);

        while (openList.Count > 0)
        {
            FlowFieldCell currentCell = openList.Dequeue();

            //get neighbours of currentCell
            foreach (FlowFieldCell neighbour in GetNeighboursFromGridIndex(currentCell.GridIndex))
            {
                //skip impassable cells
                if (neighbour.Cost == byte.MaxValue)
                    continue;

                //if normal cost + totalcost is smaller than neighbour's totalcost (either ushort.maxvalue else it probably has already been updated), update it
                if (neighbour.Cost + currentCell.BestCost < neighbour.BestCost)
                {
                    //assign total cost to current neighbour
                    neighbour.BestCost = (ushort)(neighbour.Cost + currentCell.BestCost);
                    openList.Enqueue(neighbour); //add neighbour to check all of its neighbours
                }
            }
        }

        //Source: https://leifnode.com/2013/12/flow-field-pathfinding/
        //The algorithm starts by resetting the value of all cells to a large value(I use 65535).

        //The goal node then gets its total path cost set to zero and gets added to the open list.
        //From this point the goal node is treated like a normal node.

        //The current node is made equal to the node at the beginning of the open list and gets removed from the list.

        //All of the current node’s neighbors get their total cost set to the current node’s cost plus their cost read from the cost field then they get added to the back of the open list.
        //This happens if and only if the new calculated cost is lower than the old cost.If the neighbor has a cost of 255 then it gets ignored completely.

        //This algorithm continues until the open list is empty.
    }

    public void GenerateFlowField(FlowFieldCell destinationCell)
    {
        GenerateIntegrationField(destinationCell);

        //foreach (FlowFieldDebugCell unit in _debugGrid)
        //{
        //    FlowFieldDebugCell closestNeighbour = null;
        //    float closestToGoal = float.MaxValue;
        //    foreach (FlowFieldDebugCell neighbour in unit.Neighbours)
        //    {
        //        float neighbourTotalCost = Vector3.Distance(_debugGrid[(int)_endGoalIdx.x + (int)_endGoalIdx.y * _grid.Rows].transform.position, neighbour.transform.position) + neighbour.Cost;
        //        if (closestToGoal > neighbourTotalCost)
        //        {
        //            closestNeighbour = neighbour;
        //            closestToGoal = neighbourTotalCost;
        //        }
        //    }
        //
        //    Vector3 vel = (closestNeighbour.transform.position - unit.transform.position).normalized;
        //    unit.Velocity = new Vector2(vel.x, vel.z);
        //}
    }
}