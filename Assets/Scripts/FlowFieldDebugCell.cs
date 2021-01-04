using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldDebugCell : MonoBehaviour
{
    private FlowFieldCell _cell;

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

    private void Update()
    {
        if (_renderer)
            _renderer.material.color = new Color(0, _cell.Cost / 255.0f, 0);

        if (_text)
        {
            if (_isDrawBestCost)
                _text.text = _cell.BestCost.ToString();
            else
                _text.text = _cell.Cost.ToString();
        }

        if (_arrowPivot)
        {
            if (_cell.Direction.x == 2.0f) //the 'impossible' direction-vector == endgoal
                _arrowPivot.transform.eulerAngles = new Vector3(0, 0, 90); //upright
            else
                _arrowPivot.transform.eulerAngles = new Vector3(0, Vector2.SignedAngle(_cell.Direction, Vector2.right), 0);
        }
    }
}