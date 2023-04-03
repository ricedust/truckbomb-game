using System.Collections;
using UnityEngine;

public class GameSpeedBooster : MonoBehaviour
{
    [SerializeField] private CurveLerper speedBoostCurve;
    [SerializeField] private float maxSpeedIncrease;
    
    public float currentSpeedIncrease { get; private set; }
    private void OnEnable() => PowerupManager.OnPowerupEarned += StartSpeedBoost;
    private void OnDisable() => PowerupManager.OnPowerupEarned -= StartSpeedBoost;

    private void StartSpeedBoost(float boostDurationSeconds)
    {
        // make sure there isn't already a speed boost
        if (!speedBoostCurve.enabled)
        {
            speedBoostCurve.LerpOnCurve(0, maxSpeedIncrease, boostDurationSeconds);
            StartCoroutine(UpdateSpeedIncrease());
        }
    }

    private IEnumerator UpdateSpeedIncrease()
    {
        while (speedBoostCurve.enabled)
        {
            currentSpeedIncrease = speedBoostCurve.currentValue;
            yield return null;
        }
    }
}