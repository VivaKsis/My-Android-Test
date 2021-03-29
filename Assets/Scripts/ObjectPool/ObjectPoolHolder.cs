using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public class ObjectPoolHolder : MonoBehaviour
{
	[SerializeField] private PoolData[] _initializationPoolData;
	public PoolData[] _InitializationPoolData => this._initializationPoolData;

	private void Initialize(PoolData[] initializationData)
	{
		for (int a = 0; a < initializationData.Length; a++)
		{
			Pool pool = new Pool(
				originalInstance: initializationData[a]._OriginalInstance,
				initialCapacity: initializationData[a]._InitialPoolCapacity
			);
		}
	}

    private void Awake()
    {
		ObjectPool objectPool = new ObjectPool(parent: this);
		this.Initialize(initializationData: this._initializationPoolData);
	}
}
