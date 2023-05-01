using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class RadioMinigame : Minigame, IDragHandler
{
    [SerializeField] private TextMeshPro valueText;
    [SerializeField] private Transform greenTick;
    [SerializeField] private Transform orangeTick;
    [SerializeField] private float minTickX;
    [SerializeField] private float maxTickX;
     
    [SerializeField] private float valueRange;
    [SerializeField] private float targetPlusMinusThreshold;

    private float tickRangeX;
    private float currentValue, targetValue;

    private void Awake() => tickRangeX = maxTickX - minTickX;

    public void OnDrag(PointerEventData eventData)
    {
        // dragging right increases, dragging left decreases
        if (eventData.delta.x > 0) currentValue++;
        else currentValue--;
        
        // clamp value to range
        currentValue = Mathf.Clamp(currentValue, 0, valueRange);
        // debug text
        valueText.text = (int)currentValue + " -> " + (int)targetValue;
        
        // update tick 
        orangeTick.localPosition = GetTickPosition(currentValue);
            
        if (IsCurrentValueInTargetRange()) WinMinigame();
    }
    private bool IsCurrentValueInTargetRange()
    {
        float lowerBound = targetValue - targetPlusMinusThreshold;
        float upperBound = targetValue + targetPlusMinusThreshold;

        return currentValue >= lowerBound && currentValue <= upperBound;
    }

    private Vector2 GetTickPosition(float value)
    {
        float ratio = value / valueRange;
        float tickX = minTickX + tickRangeX * ratio;
        return tickX * Vector2.right;
    }

    protected override void ResetState()
    {
        targetValue = Random.value * valueRange;
        currentValue = valueRange / 2;
        valueText.text = (int)currentValue + " -> " + (int)targetValue;

        greenTick.localPosition = GetTickPosition(targetValue);
    }
}
