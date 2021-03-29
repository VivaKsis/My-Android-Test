using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private GameObject _leftDoor;
    public GameObject _LeftDoor => _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    public GameObject _RightDoor => _rightDoor;

    [SerializeField] private TriggerZone _enterZone;
    public TriggerZone _EnterZone => _enterZone;
    [SerializeField] private TriggerZone _winZone;
    public TriggerZone _WinZone => _winZone;

    private static float LEFT_DOOR_OPEN_Z = 4.7f,
                         RIGHT_DOOR_OPEN_Z = -4.7f,
                         SPEED_STEP = 0.05f;

    private State state = State.closed;
    private float leftDoorClosePositionZ, 
                  rightDoorClosePositionZ;

    private enum State
    {
        closed,
        opened,
        closing, 
        opening
    }

    private void PlayerWins(Collider collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if(player != null)
        {
            state = State.opened;
            player.Win();
        }  
    }

    private void StartOpening(Collider collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            state = State.opening;
        }
    }
    private void StartClosing(Collider collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            state = State.closing;
        }
    }

    private void Open()
    {
        bool leftDoorOpened = false;
        Vector3 pos;

        if (_leftDoor.transform.position.z >= LEFT_DOOR_OPEN_Z)
        {
            leftDoorOpened = true;
        }
        else
        {
            pos = _leftDoor.transform.position;
            pos = new Vector3(pos.x, pos.y, pos.z + SPEED_STEP);
            _leftDoor.transform.position = pos;
        }

        if (_rightDoor.transform.position.z <= RIGHT_DOOR_OPEN_Z && leftDoorOpened)
        {
            state = State.opened;
        }
        else
        {
            pos = _rightDoor.transform.position;
            pos = new Vector3(pos.x, pos.y, pos.z - SPEED_STEP);
            _rightDoor.transform.position = pos;
        }
    }

    private void Close()
    {
        bool leftDoorClosed = false;
        Vector3 pos;

        if (_leftDoor.transform.position.z <= leftDoorClosePositionZ)
        {
            leftDoorClosed = true;
        }
        else
        {
            pos = _leftDoor.transform.position;
            pos = new Vector3(pos.x, pos.y, pos.z - SPEED_STEP);
            _leftDoor.transform.position = pos;
        }

        if (_rightDoor.transform.position.z >= rightDoorClosePositionZ && leftDoorClosed)
        {
            state = State.closed;
        }
        else
        {
            pos = _rightDoor.transform.position;
            pos = new Vector3(pos.x, pos.y, pos.z + SPEED_STEP);
            _rightDoor.transform.position = pos;
        }
    }

    public void Restart()
    {
        state = State.closing;
    }

    private void Start()
    {
        leftDoorClosePositionZ = _leftDoor.transform.position.z;
        rightDoorClosePositionZ = _rightDoor.transform.position.z;

        _enterZone.SetOnEnterAction(StartOpening);
        _enterZone.SetOnExitAction(StartClosing);

        _winZone.SetOnEnterAction(PlayerWins);
    }

    private void Update()
    {
        if(state == State.opening)
        {
            Open();
        }
        else if(state == State.closing)
        {
            Close();
        }
    }
}
