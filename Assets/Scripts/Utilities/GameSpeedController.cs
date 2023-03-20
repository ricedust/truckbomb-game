using System;
using System.Collections;
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    [SerializeField] private CurveLerper gameSpeedCurve;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float finalSpeed;
    [SerializeField] private float timeToFinalSpeedSeconds;
    [SerializeField] private float timeToStopOnGameOverSeconds;

    private Action<float> SetGameSpeed;

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

    public void Initialize(Action<float> SetGameSpeed) => this.SetGameSpeed = SetGameSpeed;
    
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
            SetGameSpeed(gameSpeedCurve.currentValue);
            yield return null;
        }
    }
}
