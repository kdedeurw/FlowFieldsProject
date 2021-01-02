using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera = null;
    [SerializeField]
    private FlowFieldGrid _grid = null;

    private float _scale = 1.0f;
    private float _originalY;

    private void Start()
    {
        if (!_camera)
            return;

        if (!_grid)
            return;

        Vector3 dimensions = _grid.Dimensions;
        _originalY = _camera.transform.position.y + dimensions.x * dimensions.y / 2;
        _camera.transform.position = new Vector3((dimensions.x * dimensions.z) / 2, _originalY, (dimensions.y * dimensions.z) / 2);
    }

    private void Update()
    {
        float change = Input.GetAxis("Mouse ScrollWheel");
        if (change != 0.0f)
        {
            _scale += change;
            if (_scale <= 0.0f)
                _scale = 0.1f;
            _camera.transform.position = new Vector3(_camera.transform.position.x, _originalY * _scale, _camera.transform.position.z);
        }
    }
}