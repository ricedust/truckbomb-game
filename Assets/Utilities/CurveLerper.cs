using System;
using UnityEngine;

/// <summary>
/// CurveLerper tracks the value of a curve over a set period of time.
/// </summary>
public class CurveLerper : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    private float startValue, endValue;
    private float currentTimeSeconds;
    private float timeToCompleteSeconds;

    /// <summary>
    /// The current value on the curve.
    /// </summary>
    public float currentValue { get; private set; }

    private void Update()
    {
        if (currentTimeSeconds < timeToCompleteSeconds)
        {
            float timeToCompleteRatio = currentTimeSeconds / timeToCompleteSeconds;

            currentValue = Mathf.LerpUnclamped(startValue, endValue, curve.Evaluate(timeToCompleteRatio));
            currentTimeSeconds += Time.deltaTime;
        }
        else enabled = false;
    }

    /// <summary>
    /// Start moving over the curve. It takes a start value, end value, and the time it will take to reach the end of the curve.
    /// </summary>
    public void LerpOnCurve(float startValue, float endValue, float timeToCompleteSeconds)
    {
        this.startValue = startValue;
        this.endValue = endValue;
        this.timeToCompleteSeconds = timeToCompleteSeconds;

        currentValue = startValue;
        currentTimeSeconds = 0;
        enabled = true;
    }

    public void ResetState()
    {
        startValue = endValue = 0;
        currentTimeSeconds = 0;
        timeToCompleteSeconds = 0;
        currentValue = 0;
    }
}