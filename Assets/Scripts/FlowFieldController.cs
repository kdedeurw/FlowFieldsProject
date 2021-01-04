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

    private void Awake()
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.NameToLayer("GridUnit")))
            {
                _flowFieldDebugManager.Grid.GetCellFromWorldPos(hitInfo.collider.transform.position).Cost += 1;
            }
        }

        //right mouse button
        if (Input.GetAxis("Mouse1") > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.NameToLayer("GridUnit")))
            {
                _flowFieldDebugManager.Grid.GetCellFromWorldPos(hitInfo.collider.transform.position).Cost = byte.MaxValue;
            }
        }

        //middle mouse button
        if (Input.GetAxis("Mouse2") > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.NameToLayer("GridUnit")))
            {
                _flowFieldDebugManager.SetEndGoal(_flowFieldDebugManager.Grid.GetCellFromWorldPos(hitInfo.collider.transform.position));
            }
        }
    }

    public Vector3 Dimensions
    {
        get { return _flowFieldDebugManager.Grid.Dimensions; }
    }
    public FlowFieldGrid Grid
    {
        get { return _flowFieldDebugManager.Grid; }
    }
}