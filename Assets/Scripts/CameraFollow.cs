using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    public GameObject _Target => _target;

    [SerializeField] private Vector3 _offset;
    public Vector3 _Offset => _offset;

    void Update()
    {
        transform.position = _target.transform.position + _offset;
    }
}
