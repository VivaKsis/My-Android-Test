using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private Action<Collider> onEnter, onExit;

    public void SetOnEnterAction(Action<Collider> onEnter)
    {
        this.onEnter += onEnter;
    }
    public void SetOnExitAction(Action<Collider> onExit)
    {
        this.onExit += onExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        onEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit?.Invoke(other);
    }
}
