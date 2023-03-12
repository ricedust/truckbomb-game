using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : StaticInstance<EnvironmentManager>
{
    [Header("References")]
    [SerializeField] private Transform background;
    [SerializeField] private SpriteRenderer backgroundSprite;

    [Header("Properties")]
    [SerializeField] private float[] laneXPositions = new float[5];

    private Vector2 initialPosition;

    /// <summary>
    /// Returns the x position of the lane at the index
    /// </summary>
    public float GetLaneX(int index) { return laneXPositions[index]; }

    public int GetNumLanes() { return laneXPositions.Length; }

    void Start()
    {
        initialPosition = background.position;
    }

    void Update()
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