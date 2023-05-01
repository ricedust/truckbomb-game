using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Numkey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite numkey;
    [SerializeField] private Sprite numkeyPressed;
    [SerializeField] private int number;

    public static event Action<int> OnNumkeyPressed;

    private void OnEnable()
    {
        spriteRenderer.sprite = numkey;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnNumkeyPressed?.Invoke(number);
        spriteRenderer.sprite = numkeyPressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.sprite = numkey;
    }
}