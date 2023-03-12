using UnityEngine;

public class ObstacleManager : StaticInstance<ObstacleManager>
{
    [Header("References")]
    [SerializeField] private ObjectPooler carPool;
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalDistanceFeet;
    [SerializeField] private float spawnYPosition;

    private float nextSpawnPointFeet;

    private void Update()
    {
        SpawnCarsInDistanceInterval();
    }

    private void SpawnCarsInDistanceInterval()
    {
        if (GameManager.instance.distanceTraveledFeet > nextSpawnPointFeet)
        {
            SpawnCar();
            nextSpawnPointFeet += spawnIntervalDistanceFeet;
        }
    }

    private void SpawnCar()
    {
        Car car = carPool.Get().GetComponent<Car>();

        car.ResetState();
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