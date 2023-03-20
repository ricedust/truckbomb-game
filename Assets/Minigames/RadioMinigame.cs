using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RadioMinigame : Minigame, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshPro valueText;
    [SerializeField] private float maxValueRange;
    [SerializeField] private float adjustmentSpeed;
    [SerializeField] private float targetPlusMinusThreshold;

    private float currentValue, targetValue;

    public void OnDrag(PointerEventData eventData)
    {
        // if dragging right, increase current value
        if (eventData.delta.x > 0) currentValue += adjustmentSpeed;
        // decrease otherwise
        else currentValue -= adjustmentSpeed;
        currentValue = Mathf.Clamp(currentValue, 0, maxValueRange);

        valueText.text = (int)currentValue + " -> " + (int)targetValue;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsCurrentValueInTargetRange()) WinMinigame();
    }

    private bool IsCurrentValueInTargetRange()
    {
        float lowerBound = targetValue - targetPlusMinusThreshold;
        float upperBound = targetValue + targetPlusMinusThreshold;

        return currentValue >= lowerBound && currentValue <= upperBound;
    }

    public override void ResetState()
    {
        targetValue = Random.Range(0, maxValueRange);
        currentValue = maxValueRange / 2;
        valueText.text = (int)currentValue + " -> " + (int)targetValue;
    }
}
