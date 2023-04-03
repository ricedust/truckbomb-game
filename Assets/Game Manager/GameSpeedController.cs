using System;
using System.Collections;
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CurveLerper gameSpeedCurve;
    [SerializeField] private GameSpeedBooster gameSpeedBooster;
    [Header("Parameters")]
    [SerializeField] private float initialSpeed;
    [SerializeField] private float finalSpeed;
    [SerializeField] private float timeToFinalSpeedSeconds;
    [SerializeField] private float timeToStopOnGameOverSeconds;

    private Action<float> SetGameSpeed;
    private float currentSpeed;

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += RampUpSpeed;
        GameManager.OnAfterStateChanged += RampDownSpeed;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= RampUpSpeed;
        GameManager.OnAfterStateChanged -= RampDownSpeed;
    }

    private void Update()
    {
        SetGameSpeed(currentSpeed + gameSpeedBooster.currentSpeedIncrease);
    }

    public void Initialize(Action<float> setGameSpeed) => SetGameSpeed = setGameSpeed;
    
    private void RampUpSpeed(GameState gameState)
    {
        if (gameState == GameState.inGame)
        {
            gameSpeedCurve.LerpOnCurve(initialSpeed, finalSpeed, timeToFinalSpeedSeconds);
            StartCoroutine(UpdateGameSpeed());
        }
    }

    private void RampDownSpeed(GameState gameState)
    {
        if (gameState == GameState.lose)
        {
            gameSpeedCurve.LerpOnCurve(gameSpeedCurve.currentValue, 0, timeToStopOnGameOverSeconds);
        }
    }

    private IEnumerator UpdateGameSpeed()
    {
        while (gameSpeedCurve.enabled)
        {
            currentSpeed = gameSpeedCurve.currentValue;
            yield return null;
        }
    }
}
