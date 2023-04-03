using System;
using System.Collections;
using UnityEngine;

public class PowerupManager : StaticInstance<PowerupManager>
{
    [SerializeField] private int boltsRequiredForPowerup;
    [SerializeField] private float powerupDurationSeconds;

    public static event Action<float> OnPowerupEarned;
    public bool isPowerupActive { get; private set; }

    private int boltsTowardPowerup;

    private void OnEnable() => Bolt.OnBoltCollected += UpdatePowerupProgress;
    private void OnDisable() => Bolt.OnBoltCollected -= UpdatePowerupProgress;

    private void UpdatePowerupProgress()
    {
        boltsTowardPowerup++;

        if (IsPowerupValid())
        {
            StartCoroutine(UpdatePowerupStatus());
            OnPowerupEarned?.Invoke(powerupDurationSeconds);
            boltsTowardPowerup = 0;
        }
    }

    private bool IsPowerupValid()
    {
        return boltsTowardPowerup >= boltsRequiredForPowerup && // meets required bolts
               !isPowerupActive && // no powerup is currently active
               GameManager.instance.state == GameState.inGame; // game is still in progress
    }

    // updates the isPowerupActive flag for duration of powerup
    private IEnumerator UpdatePowerupStatus()
    {
        isPowerupActive = true;
        yield return new WaitForSeconds(powerupDurationSeconds);
        isPowerupActive = false;
    }
}