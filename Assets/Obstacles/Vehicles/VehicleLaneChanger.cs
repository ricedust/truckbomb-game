using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleLaneChanger : MonoBehaviour
{
    [SerializeField] private PoolableObject poolableObject;
    [SerializeField] private VehicleCollision vehicleCollision;
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private CurveLerper laneChangeLerper;
    [SerializeField] private float minLaneChangeIntervalSeconds;
    [SerializeField] private float maxLaneChangeIntervalSeconds;
    [SerializeField] private float minMergeDurationSeconds;
    [SerializeField] private float maxMergeDurationSeconds;

    private int currentLane;
    
    private void OnEnable()
    {
        poolableObject.OnReset += ResetState;
    }

    private void OnDisable()
    {
        poolableObject.OnReset -= ResetState;
        StopAllCoroutines();
    }

    private void ResetState() => laneChangeLerper.ResetState();

    public void SetLane(int lane)
    {
        currentLane = lane;
        StartCoroutine(AttemptLaneChangeInterval());
    }

    public float GetCurrentX() => laneChangeLerper.enabled ? laneChangeLerper.currentValue : transform.position.x;

    private IEnumerator AttemptLaneChangeInterval()
    {
        // keep changing lanes as long as hasn't crashed
        while (vehicleCollision.collisionCount == 0)
        {
            yield return new WaitForSeconds(Random.Range(minLaneChangeIntervalSeconds, maxLaneChangeIntervalSeconds));
            if (!laneChangeLerper.enabled) AttemptLaneChange();
        }
    }

    private void AttemptLaneChange()
    {
        // add safe lanes to a list
        List<int> safeLanes = new();

        int laneToLeft = currentLane - 1;
        int laneToRight = currentLane + 1;

        if (EnvironmentManager.instance.IsValidLane(laneToLeft)
            && IsSafeToMergeDirection(Vector2.left)) safeLanes.Add(laneToLeft);

        if (EnvironmentManager.instance.IsValidLane(laneToRight)
            && IsSafeToMergeDirection(Vector2.right)) safeLanes.Add(laneToRight);

        // if safe directions exist, merge to a random, safe direction
        if (safeLanes.Any())
        {
            int newLane = safeLanes[Random.Range(0, safeLanes.Count)];
            StartLaneChange(newLane);
        }
    }

    private bool IsSafeToMergeDirection(Vector2 direction)
    {
        bool isSafe = false;

        // temporarily use the vehicle's collider to check a direction
        collider2d.isTrigger = true;
        collider2d.offset = EnvironmentManager.instance.laneWidth * direction;

        // if the collider doesn't overlap anything, it's safe to merge
        if (collider2d.OverlapCollider(new ContactFilter2D(), new List<Collider2D>()) == 0)
        {
            isSafe = true;
        }

        // restore vehicle's collider
        collider2d.offset = Vector2.zero;
        collider2d.isTrigger = false;
        return isSafe;
    }

    private void StartLaneChange(int newLane)
    {
        float targetX = EnvironmentManager.instance.laneXPositions[newLane];
        float mergeDuration = Random.Range(minMergeDurationSeconds, maxMergeDurationSeconds);

        laneChangeLerper.LerpOnCurve(transform.position.x, targetX, mergeDuration);
        currentLane = newLane;
    }
}