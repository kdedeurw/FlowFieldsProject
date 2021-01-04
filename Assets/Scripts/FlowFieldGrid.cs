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
    private FlowFieldCell _destinationCell = null;
    private byte _formerCost;

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

        for (int y = 0; y < _cols; ++y)
        {
            for (int x = 0; x < _rows; ++x)
            {
                _grid[x + y * _cols] = new FlowFieldCell(new Vector3(x * _cellSize, 0, y * _cellSize), new Vector2Int(x, y));
            }
        }
    }

    private void IdentifyAllNeighbours()
    {
        _neighbours = new List<FlowFieldCell>[_rows * _cols];

        for (int y = 0; y < _cols; ++y)
        {
            for (int x = 0; x < _rows; ++x)
            {
                int currentIdx = x + y * _rows;
                int size = 4; //4 directions
                if (_isDiagonal)
                    size = 8; //+ 4 diagonal ones
                _neighbours[currentIdx] = new List<FlowFieldCell>(size);

                //left side from current unit (== current cell - 1 in Xdir)
                if (x - 1 >= 0)
                {
                    _neighbours[currentIdx].Add(_grid[currentIdx - 1]);
                    //diagonals
                    if (_isDiagonal)
                    {
                        //left-top side (== current cell - 1 in Xdir + 1 row)
                        if (y + 1 < _cols)
                            _neighbours[currentIdx].Add(_grid[currentIdx - 1 + _rows]);
                        //left-bottom side (== current cell - 1 in Xdir - 1 row)
                        if (y - 1 >= 0)
                            _neighbours[currentIdx].Add(_grid[currentIdx - 1 - _rows]);
                    }
                }
                //right side from current unit (== current cell + 1 in Xdir)
                if (x + 1 < _rows)
                {
                    _neighbours[currentIdx].Add(_grid[currentIdx + 1]);
                    //diagonals
                    if (_isDiagonal)
                    {
                        //right-top side (== current cell + 1 in Xdir + 1 row)
                        if (y + 1 < _cols)
                            _neighbours[currentIdx].Add(_grid[currentIdx + 1 + _rows]);
                        //right-bottom side (== current cell + 1 in Xdir - 1 row)
                        if (y - 1 >= 0)
                            _neighbours[currentIdx].Add(_grid[currentIdx + 1 - _rows]);
                    }
                }

                //top side from current unit (= current cell + 1 in Ydir)
                if (y + 1 < _cols)
                    _neighbours[currentIdx].Add(_grid[currentIdx + _rows]);

                //bottom side from current unit
                if (y - 1 >= 0)
                    _neighbours[currentIdx].Add(_grid[currentIdx - _rows]);
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

    private Vector2 GetDirectionFromGridIndex(Vector2Int gridIndex)
    {
        Vector2 direction = new Vector2(0, 0);

        //pointing left or right?
        if (gridIndex.x >= 1)
            direction.x = 1;
        else if (gridIndex.x <= -1)
            direction.x = -1;
        //else if x == 0, then direction.x stays 0

        //pointing up or down?
        if (gridIndex.y >= 1)
            direction.y = 1;
        else if (gridIndex.y <= -1)
            direction.y = -1;
        //else if y == 0, then direction.y stays 0

        //if both not 0, create diagonal vector (== scale 1 or -1 with 0.707)
        if (Mathf.Abs(direction.x) != 0 && Mathf.Abs(direction.y) != 0)
        {
            //== 'normalize' vector ((-)1,(-)1)
            direction.x *= 0.707f; //will keep signed value
            direction.y *= 0.707f; //will keep signed value
        }

        return direction;
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

    public Vector2Int GetIndexFromWorldPos(Vector3 worldPos)
    {
        int xIdx = (int)(worldPos.x / _cellSize);
        int yIdx = (int)(worldPos.z / _cellSize);

        if (xIdx >= _rows || yIdx >= _cols)
            return new Vector2Int(-1, -1);

        return new Vector2Int(xIdx, yIdx);
    }

    public Vector3 GetWorldPosFromIndex(Vector2Int gridIndex)
    {
        float xPos = gridIndex.x * _cellSize; //- _cellSize / 2;
        float yPos = gridIndex.y * _cellSize; //- _cellSize / 2;

        return new Vector3(xPos, 0, yPos);
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

    private void ResetGrid()
    {
        foreach (FlowFieldCell cell in _grid)
        {
            cell.BestCost = ushort.MaxValue;
        }
        if (_destinationCell != null)
        {
            //reset former destination cell to its original cost
            _destinationCell.Cost = _formerCost;
        }
    }

    private void GenerateIntegrationField(FlowFieldCell destinationCell)
    {
        //overwrite/assign new destination cell
        _destinationCell = destinationCell;
        //save cost from destinationcell so that it cannot get lost
        _formerCost = _destinationCell.Cost;
        //since we set it to 0 here
        _destinationCell.Cost = 0;
        //bestcost can be ignored anyway
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
        ResetGrid();
        GenerateIntegrationField(destinationCell);

        foreach (FlowFieldCell currentCell in _grid)
        {
            //skip impassable and endgoal
            //if (currentCell.Cost == byte.MaxValue || currentCell.BestCost == 0) //bestcost is a better option since a cost of 0 might be possible?
            //    continue;

            //make units go 'back' from impassable terrain

            FlowFieldCell closestNeighbour = null;
            int bestCost = currentCell.BestCost;
            foreach (FlowFieldCell neighbour in GetNeighboursFromGridIndex(currentCell.GridIndex))
            {
                if (neighbour.BestCost < bestCost)
                {
                    closestNeighbour = neighbour;
                    bestCost = neighbour.BestCost;
                }
            }

            if (closestNeighbour != null)
            {
                if (currentCell.Cost == byte.MaxValue)
                    currentCell.Direction = GetDirectionFromGridIndex(currentCell.GridIndex - destinationCell.GridIndex); //destination to current (pointing away)
                else
                    currentCell.Direction = GetDirectionFromGridIndex(closestNeighbour.GridIndex - currentCell.GridIndex); //current to closest
            }
        }

        //deprecated
        //foreach (FlowFieldCell currentCell in _grid)
        //{
        //    FlowFieldCell closestNeighbour = null;
        //    float closestToGoal = float.MaxValue;
        //    foreach (FlowFieldCell neighbour in GetNeighboursFromWorldPos(currentCell.WorldPos))
        //    {
        //        float neighbourTotalCost = Vector3.Distance(destinationCell.WorldPos, neighbour.WorldPos) + neighbour.Cost;
        //        if (closestToGoal > neighbourTotalCost)
        //        {
        //            closestNeighbour = neighbour;
        //            closestToGoal = neighbourTotalCost;
        //        }
        //    }
        //
        //    Vector3 vel = (closestNeighbour.WorldPos - currentCell.WorldPos).normalized;
        //    currentCell.Direction = new Vector2(vel.x, vel.z);
        //}
    }
}