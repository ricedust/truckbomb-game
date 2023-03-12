using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int defaultPoolCapacity;
    [SerializeField] private int maxPoolCapacity;

    private ObjectPool<GameObject> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<GameObject>(() =>
        { // create function
            GameObject gameObject = Instantiate(prefab, transform);
            gameObject.GetComponent<IPoolable>().InitializeDespawnAction(DespawnAction);
            return gameObject;
        }, gameObject =>
        { // action on get
            gameObject.SetActive(true);
        }, gameObject =>
        { // action on release
            gameObject.SetActive(false);
        }, gameObject =>
        { // action on destroy
            Destroy(gameObject);
        }, false, defaultPoolCapacity, maxPoolCapacity);
    }
    
    private void DespawnAction(GameObject gameObject) => objectPool.Release(gameObject);

    public GameObject Get() => objectPool.Get();
}
