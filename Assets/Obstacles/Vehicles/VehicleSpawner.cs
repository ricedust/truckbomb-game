using System.Collections;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPooler vehiclePool;
    [SerializeField] private VehiclePicker vehiclePicker;

    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalDistanceFeet;
    [SerializeField] private float spawnYPosition;

    private float nextSpawnPointFeet;

    public void StartSpawning()
    {
        nextSpawnPointFeet = spawnIntervalDistanceFeet;
        StartCoroutine(SpawnOverDistance());
    }

    private IEnumerator SpawnOverDistance()
    {
        while (GameManager.instance.state == GameState.inGame)
        {
            if (GameManager.instance.distanceTraveledFeet > nextSpawnPointFeet)
            {
                SpawnVehicle();
                nextSpawnPointFeet += spawnIntervalDistanceFeet;
            }

            yield return null;
        }
    }

    private void SpawnVehicle()
    {
        Vehicle vehicle = vehiclePool.Get().GetComponent<Vehicle>();

        vehicle.ResetState();
        vehiclePicker.Randomize(vehicle);
        vehicle.transform.position = GenerateSpawnPosition();
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