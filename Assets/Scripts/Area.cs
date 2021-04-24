using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle;
    public GameObject _Obstacle => _obstacle;

    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;

    private static int OBSTACLE_AMOUNT = 200;
    private static float OBSTACLE_Y_COORDINATE = 0;

    private float x, z;
    
    private List<GameObject> obstacles = new List<GameObject>();
    private ObjectPool.Pool poolTransferer = new ObjectPool.Pool();

    private void FillAreaRandomly()
    {
        for (int a = 0; a < OBSTACLE_AMOUNT; a++)
        {
            obstacles.Add(poolTransferer.Aquire(_obstacle));

            x = Random.Range(_minX, _maxX);
            z = Random.Range(_minZ, _maxZ);

            obstacles[a].transform.position = new Vector3(x, OBSTACLE_Y_COORDINATE, z);
        }
    }

    public void Restart()
    {
        for(int a = 0; a < obstacles.Count; a++)
        {
            poolTransferer.Release(obstacles[a]);
        }

        FillAreaRandomly();
    }

    void Start()
    {
        FillAreaRandomly();
    }
}
