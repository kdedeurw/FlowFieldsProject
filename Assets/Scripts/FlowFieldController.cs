using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldController : MonoBehaviour
{
    [SerializeField]
    private GameObject _flowFieldDebugManagerObject = null;
    private FlowFieldDebugManager _flowFieldDebugManager;

    [SerializeField]
    private bool _isDiagonal = true;
    [SerializeField]
    private Vector2Int _dimensions = new Vector2Int(30, 30);
    [SerializeField]
    private float _cellSize = 10.0f;

    private void Start()
    {
        if (!_flowFieldDebugManager && _flowFieldDebugManagerObject)
            _flowFieldDebugManager = Instantiate(_flowFieldDebugManagerObject, transform).GetComponent<FlowFieldDebugManager>();

        if (_flowFieldDebugManager)
            _flowFieldDebugManager.CreateDebugGrid(_dimensions, _cellSize, _isDiagonal);
        else
            Debug.LogError("FlowFieldController > Start : !Unable to create debug grid!");
    }

    private void Update()
    {
        //left mouse button
        if (Input.GetAxis("Mouse0") > 0)
        {
            _flowFieldDebugManager.SetEndGoal(_flowFieldDebugManager.GetIndexFromWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition)).GridIndex);
        }
    }

    public Vector3 Dimensions
    {
        get { return _flowFieldDebugManager.Dimensions; }
    }
}