using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxCapacity;

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
        }, false, defaultCapacity, maxCapacity);
    }
    
    private void DespawnAction(GameObject gameObject) => objectPool.Release(gameObject);

    public GameObject Get() => objectPool.Get();
}
