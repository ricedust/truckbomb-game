using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CurveLerper laneChangeLerper;

    [SerializeField] [Range(0, 4)] private int targetLane;
    [SerializeField] private float timeToChangeLaneSeconds;

    private void OnEnable()
    {
        playerInput.OnPlayerMerge += ChangeLane;
    }

    private void OnDisable()
    {
        playerInput.OnPlayerMerge -= ChangeLane;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.state == GameState.inGame)
        {
            rigidbody2d.MovePosition(new Vector2(laneChangeLerper.currentValue, transform.position.y));
        }
    }

    private void ChangeLane(int lanesToMove)
    {
        float startXPosition = transform.position.x;

        targetLane += lanesToMove;
        // clamp target lane to make sure it's in range
        targetLane = Mathf.Clamp(targetLane, 0, EnvironmentManager.instance.laneXPositions.Length - 1);

        float targetXPosition = EnvironmentManager.instance.laneXPositions[targetLane];

        // start lane change animation
        laneChangeLerper.LerpOnCurve(startXPosition, targetXPosition, timeToChangeLaneSeconds);
    }
}