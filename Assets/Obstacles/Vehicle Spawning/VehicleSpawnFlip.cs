using System;
using System.Collections;
using UnityEngine;

public class VehicleSpawnFlip : MonoBehaviour
{
    [SerializeField] private VehicleSpawner spawner;

    private void OnEnable() => GameManager.OnAfterStateChanged += StartMonitoringGameSpeed;
    private void OnDisable() => GameManager.OnAfterStateChanged -= StartMonitoringGameSpeed;

    public static event Action OnSpawnFlipped;
    public static bool isSpawnFlipped { get; private set; }
    
    private void StartMonitoringGameSpeed(GameState gameState)
    {
        if (gameState == GameState.lose)
        {
            StartCoroutine(MonitorGameSpeed());
            GameManager.OnAfterStateChanged -= StartMonitoringGameSpeed;
        }
    }
    private IEnumerator MonitorGameSpeed()
    {
        // Debug.Log("vehicle speed: " + spawner.minVehiclespeed);
        // Debug.Log("game speed: " + GameManager.instance.gameSpeed);
        yield return new WaitUntil(() => spawner.minVehiclespeed > GameManager.instance.gameSpeed);
        OnSpawnFlipped?.Invoke();
        isSpawnFlipped = true;
        // Debug.Log("flipped");
    }
}