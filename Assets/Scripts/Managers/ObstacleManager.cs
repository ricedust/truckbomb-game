using UnityEngine;
using UnityEngine.Pool;

public class ObstacleManager : StaticInstance<ObstacleManager>
{
    [Header("References")]
    [SerializeField] private Car carPrefab;
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float spawnYPosition;
    [Header("Pool Parameters")]
    [SerializeField] private int defaultPoolCapacity;
    [SerializeField] private int maxPoolCapacity;

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
            // Debug.Log("Car activated");
            car.gameObject.SetActive(true);
        }, car =>
        { // action on release
            // Debug.Log("Car deactivated");
            car.gameObject.SetActive(false);
        }, car =>
        { // action on destroy
            // Debug.Log("Car destroyed");
            Destroy(car.gameObject);
        }, false, defaultPoolCapacity, maxPoolCapacity);
    }

    /// <summary>
    /// Despawn action for car objects so that cars can despawn themselves
    /// </summary>
    private void DespawnCar(Car car) => carPool.Release(car);


    public void SpawnCarsInInterval()
    {
        InvokeRepeating("SpawnCar", 0.0f, spawnIntervalInSeconds);
    }

    private void SpawnCar()
    {
        Car car = carPool.Get();

        car.Reset();
        car.transform.position = GenerateSpawnPosition();
        car.ApplyForce(Vector2.down);
    }

    /// <summary>
    /// Returns a Vector2 spawn position at the top of a random lane
    /// </summary>
    private Vector2 GenerateSpawnPosition()
    {
        int laneIndex = Random.Range(0, EnvironmentManager.instance.GetNumLanes());
        float laneXPos = EnvironmentManager.instance.GetLaneX(laneIndex);

        return new Vector2(laneXPos, spawnYPosition);
    }
}