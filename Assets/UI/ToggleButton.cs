using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ToggleButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer disabledSprite;

    [field: SerializeField] public bool isEnabled { get; private set; } = true;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        isEnabled = !isEnabled;
        disabledSprite.enabled = !isEnabled;
        FireEvent();
    }

    protected abstract void FireEvent();
}