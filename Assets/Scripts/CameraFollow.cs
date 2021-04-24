using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public Transform _Target => _target;

    [SerializeField] private Vector3 _offset;
    public Vector3 _Offset => _offset;

    void LateUpdate()
    {
        transform.position = _target.position + _offset;
        transform.LookAt(_target);
    }
}
