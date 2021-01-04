using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldDebugManager : MonoBehaviour
{
    private FlowFieldGrid _grid = null;

    [SerializeField]
    private Vector2Int _endGoalIdx = new Vector2Int(1, 1);

    [SerializeField]
    private GameObject _endGoalIndicatorTemplate = null;
    private GameObject _endGoalIndicator = null;

    [SerializeField]
    private GameObject _debugCellTemplate = null;
    private FlowFieldDebugCell[] _debugGrid = null;

    private void Start()
    {
        //TestCostFields();

        if (!_endGoalIndicator)
            _endGoalIndicator = Instantiate(_endGoalIndicatorTemplate, transform);
    }

    public void CreateDebugGrid(Vector2Int dimensions, float cellSize, bool isDiagonal)
    {
        _grid = new FlowFieldGrid(dimensions, cellSize, isDiagonal);
        _debugGrid = new FlowFieldDebugCell[_grid.Rows * _grid.Colums];

        for (int y = 0; y < _grid.Colums; ++y)
        {
            for (int x = 0; x < _grid.Rows; ++x)
            {
                GameObject gridUnit = Instantiate(_debugCellTemplate, transform);

                float posX = x * _grid.CellSize;
                float posZ = y * _grid.CellSize;

                gridUnit.transform.position = new Vector3(posX, 0, posZ);

                FlowFieldDebugCell debugCell = gridUnit.GetComponent<FlowFieldDebugCell>();
                debugCell.Cell = _grid.Cells[x + y * _grid.Rows];

                _debugGrid[x + y * _grid.Rows] = debugCell;
            }
        }
    }

    private void TestCostFields()
    {
        foreach (FlowFieldDebugCell unit in _debugGrid)
        {
            unit.Cell.Cost = (byte)Random.Range(byte.MinValue, byte.MaxValue);
        }
    }

    public void SetEndGoal(Vector2Int endGoalIdx)
    {
        _endGoalIdx = endGoalIdx;
        _grid.GenerateFlowField(_grid.GetCellFromGridIndex(endGoalIdx));

        if (_endGoalIndicator)
            _endGoalIndicator.transform.position = new Vector3(_endGoalIdx.x * _grid.CellSize, 0, _endGoalIdx.y * _grid.CellSize);
    }

    public void SetEndGoal(FlowFieldCell endGoalCell)
    {
        _endGoalIdx = endGoalCell.GridIndex;
        _grid.GenerateFlowField(endGoalCell);

        if (_endGoalIndicator)
            _endGoalIndicator.transform.position = new Vector3(_endGoalIdx.x * _grid.CellSize, 0, _endGoalIdx.y * _grid.CellSize);
    }

    public FlowFieldGrid Grid
    {
        get { return _grid; }
    }
}