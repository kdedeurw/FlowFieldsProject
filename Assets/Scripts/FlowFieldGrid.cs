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

    private List<FlowFieldGridUnit> _gridUnits = null;

    private void Start()
    {
        _gridUnits = new List<FlowFieldGridUnit>(_rows * _cols);
        GenerateGrid();
        IdentifyAllNeighbours();
        Test();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _cols; ++j)
            {
                GameObject gridUnit = Instantiate(_gridUnitTemplate, transform);

                float posX = i * _unitSize;
                float posZ = j * _unitSize;

                gridUnit.transform.position = new Vector3(posX, 0, posZ);

                _gridUnits.Add(gridUnit.GetComponent<FlowFieldGridUnit>());
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
                //right side from current unit
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
}