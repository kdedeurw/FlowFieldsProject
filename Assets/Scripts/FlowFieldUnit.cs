using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlowFieldUnit : MonoBehaviour
{
    [SerializeField]
    private float _speed = 50.0f;
    [SerializeField]
    private Rigidbody _rigidBody = null;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public float Speed
    {
        set { _speed = value; }
        get { return _speed; }
    }

    public Rigidbody RigidBody
    {
        get { return _rigidBody; }
    }
}