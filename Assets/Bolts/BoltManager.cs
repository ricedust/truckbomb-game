using System;
using UnityEngine;
using TMPro;
using System.Collections;
using Random = UnityEngine.Random;

public class BoltManager : StaticInstance<BoltManager>
{
    public int boltsCollected { get; private set; }
    
    [Header("References")]
    [SerializeField] private ObjectPooler boltPool;
    [SerializeField] private TextMeshProUGUI boltsCollectedText;
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnIntervalInSeconds;
    [SerializeField] private float spawnYPosition;

    private float[] laneXPositions;
    private int currentBoltSpawnLane;

    private void Start()
    {
        // cache lane x positions
        laneXPositions = EnvironmentManager.instance.laneXPositions;
        // initialize bolt spawn lane to the middle lane by default
        currentBoltSpawnLane = laneXPositions.Length / 2; 
    }

    private void OnEnable()
    {
        Bolt.OnBoltCollected += UpdateBoltsCollected;
        VehicleSpawner.OnVehicleWaveSpawned += ChangeBoltSpawnLane;
        GameManager.OnAfterStateChanged += ToggleBoltSpawning;
    }

    private void OnDisable()
    {
        Bolt.OnBoltCollected -= UpdateBoltsCollected;
        VehicleSpawner.OnVehicleWaveSpawned -= ChangeBoltSpawnLane;
        GameManager.OnAfterStateChanged -= ToggleBoltSpawning;
    }

    /// <summary>Starts bolt-spawning coroutine when the game starts, stop it when the game ends</summary>
    private void ToggleBoltSpawning(GameState state)
    {
        if (state == GameState.inGame) StartCoroutine(SpawnBoltsOverTime());
        else if (state == GameState.lose) StopAllCoroutines();
    }

    /// <summary>Spawns bolts on a time interval</summary>
    private IEnumerator SpawnBoltsOverTime()
    {
        while (true)
        {
            // spawn a bolt at the top of the active spawning lane
            Vector2 spawnPosition = new Vector2(laneXPositions[currentBoltSpawnLane], spawnYPosition);
            boltPool.Spawn().transform.position = spawnPosition;
            
            // wait until next interval
            yield return new WaitForSeconds(spawnIntervalInSeconds);
        }
    }

    /// <summary>Move the bolt-spawn lane to the closest lane not occupied by a vehicle</summary>
    private void ChangeBoltSpawnLane(bool[] laneAvailabilities)
    {
        // move outwards from the current lane one at a time until an available lane is found
        for (int i = 1; i < laneXPositions.Length; i++)
        {
            // check that lanes are valid and that they are available to spawn bolts in
            bool rightLaneIsValid = EnvironmentManager.instance.IsValidLane(currentBoltSpawnLane + i)
                                    && laneAvailabilities[currentBoltSpawnLane + i];
            bool leftLaneIsValid = EnvironmentManager.instance.IsValidLane(currentBoltSpawnLane - i)
                                   && laneAvailabilities[currentBoltSpawnLane - i];
            
            // if the lanes on either side are available, pick one at random
            if (rightLaneIsValid && leftLaneIsValid) currentBoltSpawnLane += Random.value < 0.5 ? i : -i;
            else if (rightLaneIsValid) currentBoltSpawnLane += i;
            else if (leftLaneIsValid) currentBoltSpawnLane -= i;
            else continue; // neither lane is valid, skip to next iteration

            return; // a valid lane has been found;
        }
    }

    private void UpdateBoltsCollected()
    {
        boltsCollected++;
        boltsCollectedText.text = "BOLTS: " + boltsCollected;
    }
}
