using System;
using UnityEngine;

public abstract class Minigame : MonoBehaviour, IPoolable
{
    public static event Action<Vector2> OnMinigameWon;

    protected Action<GameObject> Despawn;
    protected Vector2 minigamePosition;

    private void OnEnable() => GameManager.OnAfterStateChanged += DespawnIfGameOver;
    private void OnDisable() => GameManager.OnAfterStateChanged -= DespawnIfGameOver;

    public void InitializeDespawnAction(Action<GameObject> Despawn) => this.Despawn = Despawn;

    public void SetPosition(Vector2 position) => minigamePosition = transform.position = position;

    public abstract void ResetState();

    protected void WinMinigame()
    {
        OnMinigameWon?.Invoke(minigamePosition);
        Despawn(gameObject);
    }

    private void DespawnIfGameOver(GameState gameState)
    {
        if (gameState == GameState.lose) Despawn(gameObject);
    }
}