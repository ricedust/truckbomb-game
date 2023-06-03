using System;
using System.Collections;
using UnityEngine;

public class VehicleSpawnFlip : MonoBehaviour
{
    public static event Action OnSpawnFlipped;
    public static bool isSpawnFlipped { get; private set; } = true;

    [SerializeField] private VehicleSpawner spawner;

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += FlipOnStart;
        GameManager.OnAfterStateChanged += StartMonitoringGameSpeed;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= FlipOnStart;
        GameManager.OnAfterStateChanged -= StartMonitoringGameSpeed;
    }

    private void FlipOnStart(GameState gameState)
    {
        if (gameState == GameState.inGame)
        {
            isSpawnFlipped = false;
            OnSpawnFlipped?.Invoke();
        }
    }

    private void StartMonitoringGameSpeed(GameState gameState)
    {
        if (gameState == GameState.lose)
        {
            StartCoroutine(MonitorGameSpeed());
            GameManager.OnAfterStateChanged -= StartMonitoringGameSpeed;
        }
    }
    
    /// <summary>Invokes flip event when vehicle speeds exceed the game speed</summary>
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