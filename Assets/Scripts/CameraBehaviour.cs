using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera = null;
    [SerializeField]
    private FlowFieldController _controller = null;
    [SerializeField]
    private float _moveSpeed = 50.0f;

    private float _scale = 1.0f;
    private float _originalY;

    private void Start()
    {
        if (!_camera)
            return;

        if (!_controller)
            return;

        Vector3 dimensions = _controller.Dimensions;
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

        Vector3 newPos = _camera.transform.position;

        newPos.x += Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
        newPos.z += Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;

        _camera.transform.position = newPos;
    }
}