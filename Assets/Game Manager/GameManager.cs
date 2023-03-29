using System;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{
    [SerializeField] private GameSpeedController gameSpeedController;
    [SerializeField] private DistanceTracker distanceTracker;

    // events to broadcast state changes
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState state { get; private set; }
    public float gameSpeed { get; private set; }
    public float distanceTraveledFeet { get; private set; }

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
        gameSpeedController.Initialize(SetGameSpeed);
        distanceTracker.Initialize(SetDisanceTraveled);

        // call ChangeState again to enter gameplay
        ChangeState(GameState.inGame);
    }

    private void HandleInGame()
    {
        BoltManager.instance.StartSpawningBolts();
        ObstacleManager.instance.StartSpawningVehicles();
    }

    private void HandleLosing()
    {

    }

    private void SetGameSpeed(float gameSpeed) => this.gameSpeed = gameSpeed;
    private void SetDisanceTraveled(float distanceTraveledFeet) => this.distanceTraveledFeet = distanceTraveledFeet;
}

[Serializable]
public enum GameState
{
    starting = 0,
    inGame = 1,
    lose = 2,
}