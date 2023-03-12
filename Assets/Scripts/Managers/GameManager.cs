using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : StaticInstance<GameManager>
{
    [Header("References")]
    [SerializeField] private CurveLerper curveLerper;
    [SerializeField] private TextMeshProUGUI distanceTraveledText;

    [Header("Game Speed")]
    [SerializeField] private AnimationCurve gameSpeedRampingCurve;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float finalSpeed;
    [SerializeField] private float timeToFinalSpeedSeconds;
    
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
        curveLerper.LerpOnCurve(gameSpeedRampingCurve, initialSpeed, finalSpeed, timeToFinalSpeedSeconds);
        StartCoroutine(UpdateGameStats());

        // call ChangeState again to enter gameplay
        ChangeState(GameState.inGame);
    }

    private void HandleInGame()
    {
        BoltManager.instance.SpawnBoltsInInterval();
    }

    private void HandleLosing()
    {

    }
    
    private IEnumerator UpdateGameStats()
    {
        while (curveLerper.enabled)
        {
            gameSpeed = curveLerper.currentValue;
            distanceTraveledFeet += Time.deltaTime * gameSpeed * 14.7f;

            distanceTraveledText.text = "FEET TRAVELED: " + distanceTraveledFeet + "\nMILES TRAVELED: " + distanceTraveledFeet * 0.0001894f;

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