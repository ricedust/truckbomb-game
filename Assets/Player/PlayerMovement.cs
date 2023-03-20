using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CurveLerper laneChangeLerper;
    [SerializeField] [Range(0, 4)] private int targetLane;
    [SerializeField] private float timeToChangeLaneSeconds;

    private void Update()
    {
        transform.position = new Vector2(laneChangeLerper.currentValue, transform.position.y);
    }

    private void ChangeLane(int lanesToMove)
    {
        float startXPosition = transform.position.x;

        targetLane += lanesToMove;
        // make sure target lane is valid
        targetLane = Mathf.Clamp(targetLane, 0, EnvironmentManager.instance.GetNumLanes() - 1);

        float targetXPosition = EnvironmentManager.instance.GetLaneX(targetLane);

        // start lane change animation
        laneChangeLerper.LerpOnCurve(startXPosition, targetXPosition, timeToChangeLaneSeconds);
    }

    public void MergeLeft() => ChangeLane(-1);

    public void MergeRight() => ChangeLane(1);
}
