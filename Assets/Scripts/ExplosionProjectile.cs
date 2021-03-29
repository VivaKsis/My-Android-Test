using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProjectile : MonoBehaviour, IPoolable
{
    #region Object Pool
    public GameObject GameObject { get; set; }

    public ObjectPool.Pool Pool { get; set; }

    public void OnAquire()
    {
        ResetScale(_initialScale);
        isMoving = false;
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }

    #endregion

    [SerializeField] private Vector3 _initialScale = new Vector3 (0.3f, 0.3f, 0.3f);
    public Vector3 _InitialScale => _initialScale;

    [SerializeField] private Vector3 _increaseRate = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 _IncreaseRate => _increaseRate;

    [SerializeField] private float _speed = 15f;
    public float _Speed => _speed;

    private bool isMoving;
    private Vector3 destinationCoordinate;
    private Vector3 shootDirection;
    private Action onExplode;

    private void ResetScale(Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }

    private void Awake()
    {
        GameObject = this.gameObject;
        Pool = new ObjectPool.Pool();

        ResetScale(_initialScale);
    }

    public void SetOnExplode(Action onExplode)
    {
        this.onExplode += onExplode;
    }

    public void IncreaseCharge()
    {
        gameObject.transform.localScale += _increaseRate;
    }

    public void ShootFrom(Vector3 startingCoordinate)
    {
        destinationCoordinate = transform.position;
        transform.position = startingCoordinate;

        shootDirection = (destinationCoordinate - startingCoordinate).normalized;

        isMoving = true;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (isMoving && hit.gameObject.GetComponent<Obstacle>() != null)
        {
            Explode();
        }
    }

    public void Explode()
    {
        isMoving = false; 

        onExplode?.Invoke();

        Vector3 explosionPosition = transform.position;
        float radius = transform.localScale.x;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        for (int a = 0; a < colliders.Length; a++)
        {
            Obstacle obstacle = colliders[a].gameObject.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.GetExploded();
            }
        }

        Pool.Release(this);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += shootDirection * _speed * Time.deltaTime;
        }
    }
}
