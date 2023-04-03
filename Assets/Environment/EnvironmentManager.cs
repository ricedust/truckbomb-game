using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : StaticInstance<EnvironmentManager>
{
    [Header("References")]
    [SerializeField] private Transform background;
    [SerializeField] private SpriteRenderer backgroundSprite;

    [field: SerializeField] public float[] laneXPositions { get; private set; } = new float[5];
    [field: SerializeField] public float laneWidth { get; private set; }

    public bool IsValidLane(int lane) => lane >= 0 && lane < laneXPositions.Length; 
    
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = background.position;
    }

    private void Update()
    {
        float speed = GameManager.instance.gameSpeed;
        background.Translate(Vector2.down * speed * Time.deltaTime);

        if (background.position.y < -initialPosition.y)
        {
            // Debug.Log("background reset");
            background.position = initialPosition;
        }
    }
}