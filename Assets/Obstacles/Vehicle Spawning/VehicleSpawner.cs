using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObjectPooler vehiclePool;
    [SerializeField] private VehiclePicker vehiclePicker;
    [SerializeField] private VehicleSpawnInterval spawnInterval;

    [Header("Spawn Parameters")]
    [SerializeField] private float spawnYLevel;
    [SerializeField] private int minBatchSize;
    [SerializeField] private int maxBatchSize;

    [field: SerializeField] public float minVehiclespeed { get; private set; }
    [field: SerializeField] public float maxVehiclespeed { get; private set; }

    private bool[] laneAvailabilities;
    private List<Vehicle> currentBatch;

    private void OnEnable() => VehicleSpawnFlip.OnSpawnFlipped += FlipSpawning;
    private void OnDisable() => VehicleSpawnFlip.OnSpawnFlipped -= FlipSpawning;
    private void Start() => SetupLaneAvailabilities();

    private void SetupLaneAvailabilities()
    {
        // fill laneAvailabilities with true for every lane
        laneAvailabilities = new bool[EnvironmentManager.instance.laneXPositions.Length];
        ResetAvailabilities();
    }

    private void ResetAvailabilities() => Array.Fill(laneAvailabilities, true);

    private void FlipSpawning() => spawnYLevel *= -1;

    private void FixedUpdate()
    {
        bool isReadyToSpawn = spawnInterval.IsDistanceIntervalReached(spawnYLevel);
        if (isReadyToSpawn) SpawnBatchInRandomLanes();
    }

    private void SpawnBatchInRandomLanes()
    {
        // pick a random batch size
        int batchSize = Random.Range(minBatchSize, maxBatchSize + 1);
        
        for (int i = 0; i < batchSize; i++)
        {
            // get a random vehicle from the pool
            Vehicle vehicle = vehiclePool.Spawn().GetComponent<Vehicle>();
            vehiclePicker.Randomize(vehicle);
            
            // move it to an available lane
            int lane = GetRandomAvailableLane();
            Spawn(vehicle, lane);
            laneAvailabilities[lane] = false; // mark the lane as occupied
        }
        ResetAvailabilities(); // clear lane availabilities when done
    }

    private int GetRandomAvailableLane()
    {
        // collect available lanes to a list
        List<int> availableLanes = new();
        for (int i = 0; i < laneAvailabilities.Length; i++) if (laneAvailabilities[i]) availableLanes.Add(i);

        // return random lane from list
        return availableLanes[Random.Range(0, availableLanes.Count)];
    }

    private void Spawn(Vehicle vehicle, int lane)
    {
        vehicle.SetInitialPosition(lane, spawnYLevel);
        vehicle.SetInitialSpeed(Random.Range(minVehiclespeed, maxVehiclespeed));
    }
}