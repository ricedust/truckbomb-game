using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EngineMinigame : Minigame, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshPro clickCountText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int clicksRequired;
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite hitSprite;
    
    private int clickCount = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        clickCount++;
        clickCountText.text = (clicksRequired - clickCount).ToString();
        spriteRenderer.sprite = hitSprite;
        if (clickCount >= clicksRequired) WinMinigame();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.sprite = neutralSprite;
    }

    protected override void ResetState()
    {
        clickCount = 0;
        clickCountText.text = clicksRequired.ToString();
        spriteRenderer.sprite = neutralSprite;
    }
}
