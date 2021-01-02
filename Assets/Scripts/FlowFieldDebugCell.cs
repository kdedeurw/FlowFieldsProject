using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldDebugCell : MonoBehaviour
{
    private FlowFieldCell _cell;
    private Vector2 _velocity = new Vector2();

    [SerializeField]
    static bool _isDrawBestCost = true;
    [SerializeField]
    private Renderer _renderer = null;
    [SerializeField]
    private GameObject _arrowPivot = null;
    [SerializeField]
    private TextMesh _text = null;

    public FlowFieldCell Cell
    {
        set { _cell = value; }
        get { return _cell; }
    }
    public Vector2 Velocity
    {
        set { _velocity = value; }
        get { return _velocity; }
    }

    private void Update()
    {
        if (_renderer)
            _renderer.material.color = new Color(0, _cell.Cost / 255.0f, 0);

        if (_arrowPivot)
            _arrowPivot.transform.eulerAngles = new Vector3(0, Vector2.SignedAngle(_velocity, Vector2.right), 0);

        if (_text)
        {
            if (_isDrawBestCost)
                _text.text = _cell.BestCost.ToString();
            else
                _text.text = _cell.Cost.ToString();
        }
    }
}