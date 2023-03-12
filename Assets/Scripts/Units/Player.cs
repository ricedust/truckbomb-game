using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CurveLerper curveLerper;

    [Header("Properties")]
    [SerializeField] [Range(0, 4)] private int targetLane;
    [SerializeField] AnimationCurve laneChangeAnimationCurve;
    [SerializeField] private float timeToChangeLanesSeconds;
    [SerializeField] private float carCollisionForce;

    public static event Action OnCarCollision;

   private void Update()
    {
        HandleInput();
        UpdatePosition();
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
        float startXPosition = transform.position.x;

        targetLane += lanesToMove;
        // make sure target lane is valid
        targetLane = Mathf.Clamp(targetLane, 0, EnvironmentManager.instance.GetNumLanes() - 1);

        float targetXPosition = EnvironmentManager.instance.GetLaneX(targetLane);

        // start lane change animation
        curveLerper.LerpOnCurve(laneChangeAnimationCurve, startXPosition, targetXPosition, timeToChangeLanesSeconds);
    }

    private void UpdatePosition()
    {
        transform.position = new Vector2(curveLerper.currentValue, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Car car))
        {
            PushAway(car);
            OnCarCollision?.Invoke();
        }
    }

    private void PushAway(Car car)
    {
        Vector2 angleOfCollision = (car.transform.position - transform.position);
        angleOfCollision.Normalize();

        car.ApplyForce(angleOfCollision * carCollisionForce);
    }
}
