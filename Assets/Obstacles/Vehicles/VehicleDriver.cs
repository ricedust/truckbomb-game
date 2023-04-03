using System;
using System.Collections;
using UnityEngine;

public class VehicleDriver : MonoBehaviour
{
    [SerializeField] private VehicleCollision vehicleCollision;
    [SerializeField] private VehicleSensor vehicleSensor;
    [SerializeField] private VehicleLaneChanger laneChanger;
    [SerializeField] private Rigidbody2D rigidbody2d;

    [Header("Avoiding Front Collision")]
    [SerializeField] private float brakeSpeed;
    [SerializeField] private float recoverySpeed;
    
    private float initialSpeed;
    private float currentSpeed;
    
    public void SetInitialSpeed(float speed) => initialSpeed = currentSpeed = speed;

    private void FixedUpdate()
    {
        if (vehicleCollision.collisionCount == 0) // crash hasn't happened yet
        {
            AdjustSpeedAutomatically();
            Drive(currentSpeed);
        }
    }

    private void AdjustSpeedAutomatically()
    {
        // slow down to avoid front collisions
        if (vehicleSensor.IsVehicleInFrontTooClose(out float distance))
        {
            // brake harder if the vehicle in front is closer
            ChangeSpeed(-brakeSpeed / distance);
        }
        // otherwise, return to initial speed
        else if (currentSpeed < initialSpeed) ChangeSpeed(recoverySpeed);
    }
    
    private void ChangeSpeed(float changeRate) => currentSpeed += changeRate * Time.fixedDeltaTime;
    
    private void Drive(float speed)
    {
        float gameSpeed = GameManager.instance.gameSpeed;
        float deltaY = (speed - gameSpeed) * Time.fixedDeltaTime;
        
        rigidbody2d.MovePosition(new Vector2(laneChanger.GetCurrentX(), transform.position.y + deltaY));
    }
}