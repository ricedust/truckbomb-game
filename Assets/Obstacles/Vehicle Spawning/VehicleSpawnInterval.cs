using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleSpawnInterval : MonoBehaviour
{
    [SerializeField] private float distanceInterval;
    [SerializeField] private float raycastYOffset; // prevent raycast from starting inside vehicles

    private Vector2 spawnDirection = Vector2.up;
    private void OnEnable() => VehicleSpawnFlip.OnSpawnFlipped += FlipSpawnDirection;
    private void OnDisable() => VehicleSpawnFlip.OnSpawnFlipped -= FlipSpawnDirection;
    private void FlipSpawnDirection() => spawnDirection *= -1;
    
    /// <summary>Returns true when a spawn distance interval has been reached</summary>
    public bool IsDistanceIntervalReached(float spawnYLevel)
    {
        Vector2[] spawnPoints = EnvironmentManager.instance.laneXPositions
            .Select(x => new Vector2(x, spawnYLevel)).ToArray();
        
        foreach (Vector2 spawnPoint in spawnPoints)
        {
            // raycast from the top or bottom of the lane
            Vector2 raycastOrigin = spawnPoint + (-raycastYOffset * spawnDirection);
            float raycastLength = distanceInterval + raycastYOffset;
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, spawnDirection, raycastLength);
            
            // if any hit is registered, return false
            Debug.DrawRay(raycastOrigin, spawnDirection * raycastLength);
            if (hit.collider) return false;
        }
        // nothing was hit so distance interval has been reached
        return true;
    }
}