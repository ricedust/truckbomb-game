using System;
using System.Collections;
using UnityEngine;

public class GameManager : StaticInstance<GameManager>
{
    // events to broadcast state changes
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    [SerializeField] private GameSpeedController gameSpeedController;
    [SerializeField] private DistanceTracker distanceTracker;
    [SerializeField] private Player player;
    
    [SerializeField] private float postGameDelaySeconds;

    public GameState state { get; private set; }
    public float gameSpeed { get; private set; }
    public float distanceFeet { get; private set; }

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
            case GameState.postGame:
                HandlePostGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);

        Debug.Log($"New state: {newState}");
    }

    private void HandleStarting()
    {
        // give gameSpeedController and distanceTracker permission to modify game variables
        gameSpeedController.Initialize(newGameSpeed => gameSpeed = newGameSpeed);
        distanceTracker.Initialize(newDistanceFeet => distanceFeet = newDistanceFeet);
    }

    private void HandleInGame()
    {
        player.gameObject.SetActive(true);
    }

    private void HandleLosing()
    {
        StartCoroutine(DelayPostGame());
    }

    private IEnumerator DelayPostGame()
    {
        yield return new WaitForSeconds(postGameDelaySeconds);
        ChangeState(GameState.postGame);
    }

    private void HandlePostGame()
    {
    }
}

[Serializable]
public enum GameState
{
    starting = 0,
    inGame = 1,
    lose = 2,
    postGame = 3
}