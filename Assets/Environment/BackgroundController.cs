using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundSprites;
    [SerializeField] private Background bottomBackground;
    [SerializeField] private Background topBackground;
    [SerializeField] private Vector2 startPosition;

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
        // if the bottom background has left the screen
        if (backgroundQueue.Peek().transform.position.y <= -startPosition.y)
        {
            // move it to the top
            Background bottom = backgroundQueue.Dequeue();
            bottom.transform.position = startPosition;
            
            // give it a random sprite
            bottom.SetSprite(GetRandomSprite());
            
            // return it to the end of the queue
            backgroundQueue.Enqueue(bottom);
        }
    }

    private Sprite GetRandomSprite()
    {
        return backgroundSprites[Random.Range(0, backgroundSprites.Count)];
    }
}