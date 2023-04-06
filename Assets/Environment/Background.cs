using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform background;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
    private void Update() => Scroll();
    private void Scroll()
    {
        float speed = GameManager.instance.gameSpeed;
        background.Translate(speed * Time.deltaTime * Vector2.down);
    }
}
