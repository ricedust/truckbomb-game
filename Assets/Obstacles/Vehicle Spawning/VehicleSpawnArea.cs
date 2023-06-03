using UnityEngine;

public class VehicleSpawnArea : MonoBehaviour
{
    [SerializeField] private float sizeY;

    private Vector2 size;
    
    // one element array for receiving overlap results from IsEmpty()
    private readonly Collider2D[] overlapResults = new Collider2D[1];

    private void Awake()
    {
        // the box x dimension is the difference between the first and last lane
        float lastLane = EnvironmentManager.instance.laneXPositions[^1];
        float firstLane = EnvironmentManager.instance.laneXPositions[0];
        
        // assign size vector using x and y dimensions
        size = new Vector2(lastLane - firstLane, sizeY);
    }

    
    /// <summary>Returns true when a spawn distance interval has been reached</summary>
    public bool IsEmpty(float spawnYLevel)
    {
        // returns true if nothing overlaps with spawn area
        return Physics2D.OverlapBoxNonAlloc(
        new Vector2(0, spawnYLevel), // box position, based on where cars are being spawned
            size, 
        0f, // box angle
            overlapResults
        ) == 0;
    }
}