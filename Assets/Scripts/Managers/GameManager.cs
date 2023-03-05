using System;
using System.Collections;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{
    [SerializeField] private AnimationCurve gameSpeedOverTimeCurve;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speedCeiling;
    [SerializeField] private float timeToReachSpeedCeilingInSeconds;
    
    // events to broadcast state changes
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState state { get; private set; }
    public float gameSpeed { get; private set; }

    private float gameStartTime;
    
    private void Start() => ChangeState(GameState.starting); // initialize with starting state

    /// <summary>
    /// ChangeState takes a GameState enum as a parameter, 
    /// changes the state variable, 
    /// calls the appropriate functions in the switch statement, 
    /// and fires events to broadcast when the state has been changed.
    /// </summary>
    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        state = newState;
        switch (newState)
        {
            case GameState.starting:
                HandleStarting();
                break;
            case GameState.inGame:
                HandleInGame();
                break;
            case GameState.lose:
                HandleLosing();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    private void HandleStarting()
    {
        // do setup, environment, cinematics, etc
        gameStartTime = Time.time;
        gameSpeed = baseSpeed;
        StartCoroutine(UpdateGameSpeed());

        // call ChangeState again to enter gameplay
        ChangeState(GameState.inGame);
    }

    private void HandleInGame()
    {
        ObstacleManager.instance.SpawnCarsInInterval();
        BoltManager.instance.SpawnBoltsInInterval();
    }

    private void HandleLosing()
    {

    }
    
    private IEnumerator UpdateGameSpeed()
    {
        float timeElapsed = 0;
        while (timeElapsed < timeToReachSpeedCeilingInSeconds)
        {
            timeElapsed = Time.time - gameStartTime;
            float t = timeElapsed / timeToReachSpeedCeilingInSeconds;
            float curveMultiplier = speedCeiling - baseSpeed;

            gameSpeed = baseSpeed + gameSpeedOverTimeCurve.Evaluate(t) * curveMultiplier;

            yield return null;
        }
    }
}

[Serializable]
public enum GameState
{
    starting = 0,
    inGame = 1,
    lose = 2,
}