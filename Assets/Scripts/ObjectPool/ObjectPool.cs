using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class ObjectPool
{
    private static ObjectPool instance;

    private Transform poolableObjectsParent;

    public ObjectPool(ObjectPoolHolder parent)
    {
        if (instance != null)
        {
            Debug.LogWarning("ObjectPool has been already created");
        }

        instance = this;
        this.poolableObjectsParent = parent.gameObject.transform;
    }

    [Serializable]
    public struct PoolData
    {
        [SerializeField] private GameObject _originalInstance;
        public GameObject _OriginalInstance => this._originalInstance;

        [SerializeField] private int _initialPoolCapacity;
        public int _InitialPoolCapacity => this._initialPoolCapacity;

        public PoolData(GameObject originalInstance, int initialPoolCapacity)
        {
            this._originalInstance = originalInstance;
            this._initialPoolCapacity = initialPoolCapacity;
        }
    }

    public class Pool
    {
        public GameObject OriginalInstance { get; }

        public int InitialCapacity { get; }
        public int InstanceId { get; }

        private int capacity;
        private Queue<IPoolable> releasedPoolables;

        private void Populate(int quantity)
        {
            for (int b = 0; b < quantity; b++)
            {
                Transform poolableObjectTransform = Object.Instantiate(original: this.OriginalInstance).transform;
                poolableObjectTransform.parent = ObjectPool.instance.poolableObjectsParent;

                IPoolable poolable = poolableObjectTransform.GetComponent<IPoolable>();
                poolable.Pool = this;
                poolable.OnRelease();

                this.releasedPoolables.Enqueue(item: poolable);
            }
        }

        internal void ReleaseObject(IPoolable poolable)
        {
            poolable.OnRelease();

            this.releasedPoolables.Enqueue(item: poolable);
        }

        internal IPoolable AquireObject()
        {
            if (this.releasedPoolables.Count == 0)
            {
                this.capacity = (int)(this.capacity * 1.5);
                this.Populate(quantity: this.capacity);
            }

            IPoolable poolable = this.releasedPoolables.Dequeue();

            poolable.OnAquire();

            return poolable;
        }

        #region Constuctors
        public Pool(GameObject originalInstance, int initialCapacity)
        {
            this.OriginalInstance = originalInstance;

            if (initialCapacity == 0)
            {
                this.InitialCapacity = 1;
            }
            else
            {
                this.InitialCapacity = initialCapacity;
            }

            this.capacity = this.InitialCapacity;
            this.InstanceId = this.OriginalInstance.GetInstanceID();

            IPoolable originalPoolable = this.OriginalInstance.GetComponent<IPoolable>();
            originalPoolable.Pool = this;

            this.releasedPoolables = new Queue<IPoolable>(capacity: this.InitialCapacity);

            this.Populate(quantity: this.capacity);
        }
        public Pool() { }

        #endregion

        #region Public Pool Methods

        public void Release(IPoolable poolable) => poolable.Pool.ReleaseObject(poolable: poolable);

        public void Release(GameObject gameObject)
        {
            IPoolable poolable = gameObject.GetComponent<IPoolable>();

            poolable.Pool.ReleaseObject(poolable: poolable);
        }

        public GameObject Aquire(IPoolable poolable)
        {
            return poolable.Pool.AquireObject().GameObject;
        }

        public GameObject Aquire(GameObject gameObject) => this.Aquire(poolable: gameObject.GetComponent<IPoolable>());

        #endregion
    }
}
