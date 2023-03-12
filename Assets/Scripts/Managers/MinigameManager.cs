using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    // Minigame Canvas
    public Transform minigameCanvas;

    // List of minigames
    [SerializeField] Stack<Minigame> wheelStack = new Stack<Minigame>();
    [SerializeField] Stack<Minigame> engineStack = new Stack<Minigame>();
    [SerializeField] Stack<Minigame> placeHolderStack = new Stack<Minigame>();
    [SerializeField] Stack<Minigame> bombStack = new Stack<Minigame>();

    // List of Prefabs
    public Minigame wheelPrefab;

    private void OnEnable() => Player.OnCarCollision += InstantiateMinigame;
    private void OnDisable() => Player.OnCarCollision -= InstantiateMinigame;

    public void InstantiateMinigame()
    {
        Minigame miniGame = Instantiate(wheelPrefab, minigameCanvas);
        switch (miniGame.gameType)
        {
            case Minigame.Type.WHEEL:
                wheelStack.Push(miniGame);

                float yIncrement = (wheelStack.Count - 1) * 0.25f;
                miniGame.transform.position = new Vector2(-4, -yIncrement);
                //instantiated.transform.parent = minigameCanvas.transform;
                break;
            case Minigame.Type.ENGINE:
                break;
            case Minigame.Type.PLACEHOLDER:
                break;
            case Minigame.Type.BOMB:
                break;
        }

        Debug.Log(miniGame.name + " instantiated!");
    }
}
