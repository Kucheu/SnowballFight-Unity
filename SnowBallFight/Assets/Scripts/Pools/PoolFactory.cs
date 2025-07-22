using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolFactory : MonoBehaviourSingleton<PoolFactory>
{
    [SerializeField] 
    private int defaultCapacity = 10;
    [SerializeField] 
    private int maxPoolSize = 100;
    [SerializeField]
    private bool collectionCheck = true;

    private Dictionary<PoolObjectType, IObjectPool<PoolObject>> pools;

    protected override void Awake()
    {
        base.Awake();
        pools = new();
    }

    public PoolObject GetObject(PoolObjectSettings setting)
    {
        IObjectPool<PoolObject> pool = GetPool(setting);
        return pool.Get();
    }

    public void ReleaseObject(PoolObject poolObject)
    {
        IObjectPool<PoolObject> pool = GetPool(poolObject.settings);
        pool.Release(poolObject);
    }

    private IObjectPool<PoolObject> GetPool(PoolObjectSettings poolSettings)
    {
        IObjectPool<PoolObject> pool;

        if (pools.TryGetValue(poolSettings.Type, out pool)) return pool;

        pool = new ObjectPool<PoolObject>(
                poolSettings.Create,
                poolSettings.OnGet,
                poolSettings.OnRelease,
                poolSettings.OnDestroyPoolObject,
                collectionCheck: this.collectionCheck,
                defaultCapacity: this.defaultCapacity,
                maxSize: maxPoolSize
            );
        pools.Add(poolSettings.Type, pool);
        return pool;
    }
}