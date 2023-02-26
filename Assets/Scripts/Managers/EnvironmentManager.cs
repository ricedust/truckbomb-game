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
    [SerializeField] private float speed = 1;

    private Vector3 initialPosition; // made this a Vector3 to preserve the depth of the background
    private float finalYPosition;

    /// <summary>Returns the x position of the lane at a certain index</summary>
    public float GetLaneX(int index) { return laneXPositions[index]; }
    
    /// <summary>Returns the number of lanes</summary>
    public int GetNumLanes() { return laneXPositions.Length; }

    void Start()
    {
        initialPosition = background.position;
        finalYPosition = -initialPosition.y;
    }

    void Update()
    {
        background.Translate(Vector2.down * speed * Time.deltaTime);
        if (background.position.y < finalYPosition)
        {
            Debug.Log("background reset");
            background.position = initialPosition;
        }
    }
}