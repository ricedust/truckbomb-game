using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundSprites;
    [SerializeField] private Background bottomBackground;
    [SerializeField] private Background topBackground;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition; 

    private Queue<Background> backgroundQueue = new();

    private void Awake()
    {
        // give the top and bottom backgrounds random sprites and queue them
        bottomBackground.SetSprite(GetRandomSprite());
        backgroundQueue.Enqueue(bottomBackground);

        topBackground.SetSprite((GetRandomSprite()));
        backgroundQueue.Enqueue(topBackground);
    }

    private void Update()
    {
        ScrollBackgrounds();
        // reset bottom background once it has fully left the screen
        if (backgroundQueue.Peek().transform.position.y <= endPosition.y)
        {
            ResetBottomBackground();
        }
    }

    private void ScrollBackgrounds()
    {
        float distance = GameManager.instance.gameSpeed * Time.deltaTime;
        bottomBackground.Scroll(distance);
        topBackground.Scroll(distance);
    }

    private void ResetBottomBackground()
    {
        // dequeue background
        Background bottom = backgroundQueue.Dequeue();

        // give it a random sprite
        bottom.SetSprite(GetRandomSprite());

        // the background probably overshot the end position, so record this difference
        float gap = endPosition.y - bottom.transform.position.y;
            
        // move the background to the top, accounting for gap
        bottom.transform.position = startPosition + (gap * Vector2.down);

        // return the background to queue
        backgroundQueue.Enqueue(bottom);
    }
    
    private Sprite GetRandomSprite()
    {
        return backgroundSprites[Random.Range(0, backgroundSprites.Count)];
    }
}