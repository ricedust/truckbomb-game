using UnityEngine;

public class EffectMover : MonoBehaviour
{
    [SerializeField] private float percentGameSpeed;
    void Update()
    {
        float gameSpeed = GameManager.instance.gameSpeed * (percentGameSpeed / 100.0f);
        transform.Translate(gameSpeed * Time.deltaTime * Vector2.down);
    }
}