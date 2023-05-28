using System;
using System.Collections;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    private Action<float> SetDistanceTraveled;
    private const float feetPerUnit = 14.7f;

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += StartTracking;
        GameManager.OnAfterStateChanged += UpdateHighScore;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= StartTracking;
        GameManager.OnAfterStateChanged -= UpdateHighScore;
    }

    public void Initialize(Action<float> SetDistanceTraveled) => this.SetDistanceTraveled = SetDistanceTraveled;
    
    private void StartTracking(GameState state)
    {
        if (state != GameState.inGame) return;
        StartCoroutine(UpdateDistance());
    }

    private IEnumerator UpdateDistance()
    {
        // update as long as game speed isn't zero or in game
        while (GameManager.instance.gameSpeed > 0 
               || GameManager.instance.state == GameState.inGame)
        {
            float distanceFeet = GameManager.instance.distanceFeet;
            distanceFeet += Time.deltaTime * GameManager.instance.gameSpeed * feetPerUnit;
            SetDistanceTraveled(distanceFeet);
            yield return null;
        }
    }

    private void UpdateHighScore(GameState state)
    {
        if (state != GameState.postGame) return;
        
        if (GameManager.instance.distanceFeet > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", GameManager.instance.distanceFeet);
            PlayerPrefs.Save();
        }
    }
}
