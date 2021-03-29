using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolable
{
    #region ObjectPool

    public GameObject GameObject { get; set; }

    public ObjectPool.Pool Pool { get; set; }

    public void OnAquire()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    #endregion

    [SerializeField] private bool _destructible;
    public bool _Destructible => _destructible;

    private void Awake()
    {
        GameObject = this.gameObject;
        Pool = new ObjectPool.Pool();
    }

    public void GetExploded()
    {
        if (_destructible)
        {
            Pool.Release(this);
        }
    }
}
