using UnityEngine;
using UnityEngine.Pool;

public class ObstacleManager : StaticInstance<ObstacleManager>
{
    [Header("References")]
    [SerializeField] private Car carPrefab;

    [Header("Properties")]
    [SerializeField] private float spawnIntervalSeconds = 1.0f;
    [SerializeField] private float spawnYPosition = 4.0f;

    private ObjectPool<Car> carPool;

    private void Start()
    {
        carPool = new ObjectPool<Car>(() =>
        { // create function
            Car car = Instantiate(carPrefab, transform);
            car.Initialize(DespawnCar);
            return car;
        }, car =>
        { // action on get
            Debug.Log("Car activated");
            car.gameObject.SetActive(true);
        }, car =>
        { // action on release
            Debug.Log("Car deactivated");
            car.gameObject.SetActive(false);
        }, car =>
        { // action on destroy
            Debug.Log("Car destroyed");
            Destroy(car.gameObject);
        }, false, 20, 40);
    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnCar", 0.0f, spawnIntervalSeconds);
    }

    private void SpawnCar()
    {
        int laneIndex = Random.Range(0, EnvironmentManager.instance.GetNumLanes());
        float lanePos = EnvironmentManager.instance.GetLaneX(laneIndex);

        Car car = carPool.Get();
        // reset transform
        car.transform.position = new Vector2(lanePos, spawnYPosition);
        car.transform.eulerAngles = Vector2.zero;
    }

    private void DespawnCar(Car car)
    {
        carPool.Release(car);
    }
}