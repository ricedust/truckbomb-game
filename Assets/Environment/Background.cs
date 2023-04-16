using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform background;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
    public void Scroll(float distance)
    {
        background.Translate(distance * Vector2.down);
    }
}
