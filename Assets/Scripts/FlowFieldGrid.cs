using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldGrid : MonoBehaviour
{
    [SerializeField]
    private bool _isDiagonal = true;
    [SerializeField]
    private int _rows = 20;
    [SerializeField]
    private int _cols = 40;
    [SerializeField]
    private float _unitSize = 10.0f;
    [SerializeField]
    private GameObject _gridUnitTemplate = null;

    [SerializeField]
    private Vector2 _endGoalIdx;
    [SerializeField]
    private GameObject _endGoalIndicatorTemplate = null;

    private GameObject _endGoalIndicator = null;

    private FlowFieldGridUnit[] _gridUnits;

    private void Start()
    {
        _gridUnits = new FlowFieldGridUnit[_rows * _cols];
        GenerateGrid();
        IdentifyAllNeighbours();
        UpdateGrid();
        //Test();

        if (!_endGoalIndicator)
            _endGoalIndicator = Instantiate(_endGoalIndicatorTemplate, transform);
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _cols; ++j)
            {
                GameObject gridUnit = Instantiate(_gridUnitTemplate, transform);

                //row-based
                float posX = j * _unitSize;
                float posZ = i * _unitSize;
                //column based: i first

                gridUnit.transform.position = new Vector3(posX, 0, posZ);

                _gridUnits[i + j * _rows] = gridUnit.GetComponent<FlowFieldGridUnit>();
            }
        }
    }

    private void IdentifyAllNeighbours()
    {
        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _cols; ++j)
            {
                int currentIdx = i + j * _rows;

                //left side from current unit
                if (i - 1 >= 0)
                {
                    _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i - 1 + j * _rows]);
                    //diagonals
                    if (_isDiagonal)
                    {
                        //left-top side
                        if (j + 1 < _cols)
                            _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i - 1 + (j + 1) * _rows]);
                        //left-bottom side
                        if (j - 1 >= 0)
                            _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i - 1 + (j - 1) * _rows]);
                    }
                }
                //right side from current unit ACTUALLY TOP SIDE
                if (i + 1 < _rows)
                {
                    _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i + 1 + j * _rows]);
                    //diagonals
                    if (_isDiagonal)
                    {
                        //right-top side
                        if (j + 1 < _cols)
                            _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i + 1 + (j + 1) * _rows]);
                        //right-bottom side
                        if (j - 1 >= 0)
                            _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i + 1 + (j - 1) * _rows]);
                    }
                }

                //top side from current unit
                if (j + 1 < _cols)
                    _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i + (j + 1) * _rows]);

                //bottom side from current unit
                if (j - 1 >= 0)
                    _gridUnits[currentIdx].Neighbours.Add(_gridUnits[i + (j - 1) * _rows]);
            }
        }
    }

    private void Test()
    {
        foreach (FlowFieldGridUnit unit in _gridUnits)
        {
            unit.Cost = Random.Range(0, 255);
        }
    }

    private void UpdateGrid()
    {
        foreach (FlowFieldGridUnit unit in _gridUnits)
        {
            FlowFieldGridUnit closestNeighbour = null;
            float closestToGoal = float.MaxValue;
            foreach (FlowFieldGridUnit neighbour in unit.Neighbours)
            {
                float neighbourTotalCost = Vector3.Distance(_gridUnits[(int)_endGoalIdx.x + (int)_endGoalIdx.y * _rows].transform.position, neighbour.transform.position) + neighbour.Cost;
                if (closestToGoal > neighbourTotalCost)
                {
                    closestNeighbour = neighbour;
                    closestToGoal = neighbourTotalCost;
                }
            }

            Vector3 vel = (closestNeighbour.transform.position - unit.transform.position).normalized;
            unit.Velocity = new Vector2(vel.x, vel.z);
        }
    }

    //returns X == amount of rows, Y == amount of cols and Z == unit size
    public Vector3 Dimensions
    {
        get { return new Vector3(_rows, _cols, _unitSize); }
    }

    private void Update()
    {
        //UpdateGrid();

        _endGoalIndicator.transform.position = new Vector3(_endGoalIdx.x * _unitSize, 0, _endGoalIdx.y * _unitSize);
    }
}