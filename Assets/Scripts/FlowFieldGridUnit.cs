using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldGridUnit : MonoBehaviour
{
    [SerializeField]
    private bool _isImpassable = false;
    [SerializeField]
    private int _cost = 0;
    [SerializeField]
    private TextMesh _text = null;

    private Vector2 _velocity = new Vector2();

    private List<FlowFieldGridUnit> _neighbours = new List<FlowFieldGridUnit>();

    [SerializeField]
    private Renderer _renderer = null;
    [SerializeField]
    private GameObject _arrowPivot = null;

    public List<FlowFieldGridUnit> Neighbours
    {
        set { _neighbours = value; }
        get { return _neighbours; }
    }
    public int Cost
    {
        set { _cost = value; }
        get { return _cost; }
    }
    public bool Impassable
    {
        set { _isImpassable = value; }
        get { return _isImpassable; }
    }
    public Vector2 Velocity
    {
        set { _velocity = value; }
        get { return _velocity; }
    }

    private void Update()
    {
        if (_renderer)
            _renderer.material.color = new Color(_cost / 255.0f, 0, 0);

        if (_arrowPivot)
            _arrowPivot.transform.eulerAngles = new Vector3(0, Vector2.SignedAngle(_velocity, Vector2.right), 0);

        if (_text)
            _text.text = _velocity.ToString();
    }
}