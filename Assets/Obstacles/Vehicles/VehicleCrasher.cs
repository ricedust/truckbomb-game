using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleCrasher : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private PoolableObject poolableObject;
    [SerializeField] private VehicleCollision vehicleCollision;
    [Header("Crash Brakes")]
    [SerializeField] private float brakingForce;
    [SerializeField] private float brakingDrag;
    [SerializeField] private float brakingAngularDrag;
    [Header("Panic Steering")]
    [SerializeField] private float panicSteeringTorqueMultiplier;
    [SerializeField] private float panicSteeringForce;

    private float noiseSeed; // for unique panic steering behavior
    private void OnEnable()
    {
        vehicleCollision.OnCollision += EnableCrashPhysics;
        poolableObject.OnReset += ResetPhysics;
    }
    private void OnDisable()
    {
        vehicleCollision.OnCollision -= EnableCrashPhysics;
        poolableObject.OnReset -= ResetPhysics;
    }
    
    private void FixedUpdate()
    {
        // if vehicle has been hit, crash
        if (vehicleCollision.collisionCount > 0)
        {
            ApplyPanicSteering();
            ApplyBrakes();
        }
    }

    public void ApplyForce(Vector2 forceVector)
    {
        rigidbody2d.AddForce(forceVector, ForceMode2D.Impulse);
    }
    
    public void ApplyBrakes()
    {
        // illusion only works while road is moving
        if (!VehicleSpawnFlip.isSpawnFlipped)
        {
            float gameSpeed = GameManager.instance.gameSpeed;
            rigidbody2d.AddForce(brakingForce * Time.fixedDeltaTime * Vector2.down);

            // clamp downward velocity to the game speed
            if (rigidbody2d.velocity.y < -gameSpeed)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -gameSpeed);
            }
        }
    }
    
    public void ApplyPanicSteering()
    {
        // noise value between -1 and 1
        float noise = Mathf.PerlinNoise(noiseSeed + Time.time, 0) * (Random.value * 2 - 1);

        rigidbody2d.AddTorque(noise * panicSteeringTorqueMultiplier * Time.fixedDeltaTime);
        rigidbody2d.AddRelativeForce(panicSteeringForce * Time.fixedDeltaTime * Vector2.up);
    }
    
    private void EnableCrashPhysics()
    {
        rigidbody2d.drag = brakingDrag;
        rigidbody2d.angularDrag = brakingAngularDrag;
        vehicleCollision.OnCollision -= EnableCrashPhysics;
    }

    private void ResetPhysics()
    {
        noiseSeed = Random.value * 1000;
        
        transform.eulerAngles = Vector2.zero;
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.drag = 0;
        rigidbody2d.angularDrag = 0;
    }
}