using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
    public CinemachineFreeLook _CinemachineFreeLook => _cinemachineFreeLook;

    private static float DISTANCE_SPEED = 1f,
                         Y_ROTATION_SPEED = 10f,
                         X_ROTATION_SPEED = 10f,
                         Y_MIN_VALUE = 0.15f;

    private void CameraRotate()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log(Input.GetAxis("Mouse X"));
            _cinemachineFreeLook.m_YAxis.Value -= Input.GetAxis("Mouse Y") / Y_ROTATION_SPEED;
            _cinemachineFreeLook.m_XAxis.Value += Input.GetAxis("Mouse X") * X_ROTATION_SPEED;
        }
    }

    private void CameraDistance()
    {
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel > 0)
        {
            _cinemachineFreeLook.m_Orbits[0].m_Height -= DISTANCE_SPEED;
            _cinemachineFreeLook.m_Orbits[0].m_Radius -= DISTANCE_SPEED;
        }
        else if (mouseScrollWheel < 0)
        {
            _cinemachineFreeLook.m_Orbits[0].m_Height += DISTANCE_SPEED;
            _cinemachineFreeLook.m_Orbits[0].m_Radius += DISTANCE_SPEED;
        }
    }

    private void LateUpdate()
    {
        if(_cinemachineFreeLook.m_YAxis.Value <= 0)
        {
            _cinemachineFreeLook.m_YAxis.Value = Y_MIN_VALUE;
        }

        CameraRotate();

        CameraDistance();
    }
}
