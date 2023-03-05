using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] [Range(0, 4)] private int targetLane;
    [SerializeField] private AnimationCurve laneChangeAnimationCurve;
    [SerializeField] private float laneChangeSpeed;
    [SerializeField] private float collisionPower;

    public static event Action OnCollision;

    private float startX, targetX;
    private float animationTimer; // the x-axis of the animation curve

    private void Start()
    {
        animationTimer = 1; // animation is complete at beginning of game
    }

    private void Update()
    {
        HandleInput();
        if (animationTimer < 1) LerpToLane();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
    }
    private void ChangeLane(int lanesToMove)
    {
        // take a snapshot of the current pos
        startX = transform.position.x;

        // set target lane and make sure it is valid
        targetLane += lanesToMove;
        targetLane = Mathf.Clamp(targetLane, 0, EnvironmentManager.instance.GetNumLanes() - 1);

        // reset animation timer
        animationTimer = 0.0f;
    }

    private void LerpToLane()
    {
        // set the targetX
        targetX = EnvironmentManager.instance.GetLaneX(targetLane);

        // lerp based on curve
        float currentX = Mathf.LerpUnclamped(startX, targetX, laneChangeAnimationCurve.Evaluate(animationTimer));
        transform.position = new Vector2(currentX, transform.position.y);

        // increment the animation timer
        animationTimer += laneChangeSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Car car))
        {
            PushAway(car);
            OnCollision?.Invoke();
        }
    }

    private void PushAway(Car car)
    {
        Vector2 angleOfCollision = (car.transform.position - transform.position);
        angleOfCollision.Normalize();

        car.ApplyForce(angleOfCollision * collisionPower);
    }
}
