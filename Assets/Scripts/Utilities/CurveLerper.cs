using UnityEngine;

/// <summary>
/// CurveLerper tracks the value of a curve over a set period of time.
/// </summary>
public class CurveLerper : MonoBehaviour
{
    private AnimationCurve curve;
    private float startValue, endValue;
    private float currentTimeSeconds;
    private float timeToCompleteSeconds;

    /// <summary>
    /// The current value on the curve.
    /// </summary>
    public float currentValue { get; private set; }

    private void Update()
    {
        // Debug.Log(currentValue);
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
    public void LerpOnCurve(AnimationCurve curve, float startValue, float endValue, float timeToCompleteSeconds)
    {
        this.curve = curve;
        this.startValue = startValue;
        this.endValue = endValue;
        this.timeToCompleteSeconds = timeToCompleteSeconds;

        enabled = true;
        currentTimeSeconds = 0;
    }
}
