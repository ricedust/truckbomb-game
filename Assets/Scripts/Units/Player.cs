using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] [Range(0, 4)] private int targetLane = 2;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float moveSpeed = 5.0f;

    private float startX, targetX;
    private float t; // t is the x-axis of the animation curve

    private void Start()
    {
        startX = targetX = EnvironmentManager.instance.GetLaneX(targetLane);
        t = 1.0f;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // on input, take a snapshot of the current pos, set the target lane, and reset animation timer
        if (Input.GetKeyDown(KeyCode.A))
        {
            startX = transform.position.x;
            targetLane--;
            t = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            startX = transform.position.x;
            targetLane++;
            t = 0.0f;
        }

        // clamp the target lane and set the targetX
        targetLane = Mathf.Clamp(targetLane, 0, EnvironmentManager.instance.GetNumLanes() - 1);
        targetX = EnvironmentManager.instance.GetLaneX(targetLane);

        // increment the animation timer, lerp based on the animation curve
        if (t < 1.0f) t += moveSpeed * Time.deltaTime;
        transform.position = Vector2.LerpUnclamped(startX * Vector2.right, targetX * Vector2.right, curve.Evaluate(t));
    }
}
