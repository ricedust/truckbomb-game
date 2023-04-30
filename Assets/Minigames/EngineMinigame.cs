using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EngineMinigame : Minigame, IPointerDownHandler
{
    [SerializeField] private TextMeshPro clickCountText;
    [SerializeField] private int clicksRequired;
    
    private int clickCount = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        clickCount++;
        clickCountText.text = (clicksRequired - clickCount).ToString();
        if (clickCount >= clicksRequired) WinMinigame();
    }

    protected override void ResetState()
    {
        clickCount = 0;
        clickCountText.text = clicksRequired.ToString();
    }
}
