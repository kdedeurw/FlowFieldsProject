using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldGridUnit : MonoBehaviour
{
    [SerializeField]
    private bool _isImpassable = false;
    [SerializeField]
    private int _cost = 0;

    private List<FlowFieldGridUnit> _neighbours = new List<FlowFieldGridUnit>();

    [SerializeField]
    private Renderer _renderer = null;

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

    private void Update()
    {
        if (_renderer)
            _renderer.material.color = new Color(_cost / 255.0f, 0, 0);
    }
}