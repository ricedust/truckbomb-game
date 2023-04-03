using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private PoolableObject prefab;
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxSize;

    private ObjectPool<PoolableObject> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<PoolableObject>(() =>
        {
            // create function
            PoolableObject poolableObject = Instantiate(prefab, transform);
            poolableObject.InitializeDespawnAction(Despawn);
            return poolableObject;
        }, poolableObject =>
        {
            // action on get
            poolableObject.gameObject.SetActive(true);
        }, poolableObject =>
        {
            // action on release
            poolableObject.gameObject.SetActive(false);
        }, poolableObject =>
        {
            // action on destroy
            Destroy(poolableObject.gameObject);
        }, false, defaultCapacity, maxSize);
    }

    private void Despawn(PoolableObject poolableObject) => objectPool.Release(poolableObject);

    /// <summary>Activate and reset an object from the pool</summary>
    public GameObject Spawn()
    {
        PoolableObject poolableObject = objectPool.Get();
        poolableObject.Reset();
        return poolableObject.gameObject;
    }
}