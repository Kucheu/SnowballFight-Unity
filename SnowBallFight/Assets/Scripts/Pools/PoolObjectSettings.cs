using UnityEngine;

[CreateAssetMenu(menuName = "Pool/DefaultPoolObjectSettings")]
public abstract class PoolObjectSettings : ScriptableObject
{
    public PoolObjectType Type;
    public GameObject prefab;

    public virtual PoolObject Create()
    {
        var gameObject = Instantiate(prefab);
        gameObject.SetActive(false);
        gameObject.name = prefab.name;
        var poolObject = gameObject.GetComponent<PoolObject>();
        poolObject.SetSettings(this);
        return poolObject;
    }

    public virtual void OnGet(PoolObject poolObject) => poolObject.gameObject.SetActive(true);
    public virtual void OnRelease(PoolObject poolObject) => poolObject.gameObject.SetActive(false);
    public virtual void OnDestroyPoolObject(PoolObject poolObject) => Destroy(poolObject.gameObject);

}
