using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldDebugManager : MonoBehaviour
{
    private FlowFieldGrid _grid = null;

    [SerializeField]
    private Vector2Int _endGoalIdx = new Vector2Int();

    [SerializeField]
    private GameObject _endGoalIndicatorTemplate = null;
    private GameObject _endGoalIndicator = null;

    [SerializeField]
    private GameObject _debugCellTemplate = null;
    private FlowFieldDebugCell[] _debugGrid = null;

    private void Start()
    {
        Test();

        if (!_endGoalIndicator)
            _endGoalIndicator = Instantiate(_endGoalIndicatorTemplate, transform);
    }

    public void CreateDebugGrid(Vector2Int dimensions, float cellSize, bool isDiagonal)
    {
        _grid = new FlowFieldGrid(dimensions, cellSize, isDiagonal);
        _debugGrid = new FlowFieldDebugCell[_grid.Rows * _grid.Colums];

        for (int i = 0; i < _grid.Rows; ++i)
        {
            for (int j = 0; j < _grid.Colums; ++j)
            {
                GameObject gridUnit = Instantiate(_debugCellTemplate, transform);

                //row-based
                float posX = j * _grid.CellSize;
                float posZ = i * _grid.CellSize;
                //column based: i first

                gridUnit.transform.position = new Vector3(posX, 0, posZ);

                FlowFieldDebugCell debugCell = gridUnit.GetComponent<FlowFieldDebugCell>();
                debugCell.Cell = _grid.Cells[i + j * _grid.Rows];

                _debugGrid[i + j * _grid.Rows] = debugCell;
            }
        }
    }

    private void Test()
    {
        foreach (FlowFieldDebugCell unit in _debugGrid)
        {
            unit.Cell.Cost = (byte)Random.Range(byte.MinValue, byte.MaxValue);
        }
    }

    private void Update()
    {
        if (_endGoalIndicator)
            _endGoalIndicator.transform.position = new Vector3(_endGoalIdx.x * _grid.CellSize, 0, _endGoalIdx.y * _grid.CellSize);
    }

    public void SetEndGoal(Vector2Int endGoalIdx)
    {
        _endGoalIdx = endGoalIdx;
    }

    public FlowFieldCell GetIndexFromWorldPos(Vector3 worldPos)
    {
        return _grid.GetCellFromWorldPos(worldPos);
    }

    public Vector3 Dimensions
    {
        get { return _grid.Dimensions; }
    }
}