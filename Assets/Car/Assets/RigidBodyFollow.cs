using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Rigidbody _rb;
    private Vector3 _localPositionOffset;
    private Vector3 _localRotationOffset;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _localPositionOffset = transform.position - _target.position;
        _localRotationOffset = transform.rotation.eulerAngles - _target.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_target.position + _localPositionOffset);
        _rb.MoveRotation(Quaternion.Euler(_target.rotation.eulerAngles + _localRotationOffset));
    }
}
