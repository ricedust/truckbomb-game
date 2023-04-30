using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Minigame : MonoBehaviour
{
    [SerializeField] private PoolableObject poolableObject;
    public static event Action<Vector2> OnMinigameWon;

    private Vector2 minigamePosition;
    public void SetPosition(Vector2 position) => minigamePosition = transform.position = position;

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += DespawnIfGameOver;
        poolableObject.OnReset += ResetState;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= DespawnIfGameOver;
        poolableObject.OnReset -= ResetState;
    }

    private void DespawnIfGameOver(GameState gameState)
    {
        if (gameState == GameState.lose) poolableObject.Despawn();
        GameManager.OnAfterStateChanged -= DespawnIfGameOver;
    }

    protected abstract void ResetState();

    protected void WinMinigame()
    {
        OnMinigameWon?.Invoke(minigamePosition);
        poolableObject.Despawn();
    }
}