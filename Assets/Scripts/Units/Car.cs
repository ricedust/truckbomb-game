using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;
    [Header("Parameters")]
    [SerializeField] private float speedRelativeToGameSpeed;
    [Header("Braking")]
    [SerializeField] private float brakingForce;
    [SerializeField] [Range(0, 1)] private float brakingDrag;
    [SerializeField] [Range(0, 1)] private float brakingAngularDrag;
    [Header("Panic Steering")]
    [SerializeField] private float panicSteeringTorqueMultiplier;
    [SerializeField] private float panicSteeringForce;

    private Action<Car> DespawnAction;
    private bool hasCrashed;

    public void FixedUpdate()
    {
        if (!hasCrashed) Drive();
        else
        {
            ApplyPanicSteering();
            ApplyBrakes();
        }
    }

    /// <summary>
    /// Attach despawn action to car object to release itself from the pool later
    /// </summary>
    public void Initialize(Action<Car> DespawnAction)
    {
        this.DespawnAction = DespawnAction;
    }

    public void ApplyForce(Vector2 forceVector)
    {
        rigidbody2d.AddForce(forceVector, ForceMode2D.Impulse);
    }

    private void Drive()
    {
        float gameSpeed = GameManager.instance.gameSpeed;

        rigidbody2d.velocity = Vector2.down * gameSpeed + Vector2.up * speedRelativeToGameSpeed;
    }

    private void Crash()
    {
        hasCrashed = true;
        rigidbody2d.drag = brakingDrag;
        rigidbody2d.angularDrag = brakingAngularDrag;
    }

    private void ApplyBrakes()
    {
        float gameSpeed = GameManager.instance.gameSpeed;
        rigidbody2d.AddForce(Vector2.down * brakingForce * Time.deltaTime);

        // clamp downward velocity to the game speed
        if (rigidbody2d.velocity.y < -gameSpeed)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -gameSpeed);
        }
    }
    private void ApplyPanicSteering()
    {
        // nosie value between -1 and 1
        float noise = Mathf.PerlinNoise(Time.time * Random.value, 0) * (Random.value * 2 - 1);

        rigidbody2d.AddTorque(noise * panicSteeringTorqueMultiplier * Time.deltaTime);
        rigidbody2d.AddRelativeForce(Vector2.up * panicSteeringForce * Time.deltaTime);
    }

    public void Reset()
    {
        hasCrashed = false;

        transform.eulerAngles = Vector2.zero;

        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0;
        rigidbody2d.drag = 0;
        rigidbody2d.angularDrag = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This is the logic for despawning cars after two collisions
        // Commented out temporarily because it's funny to watch cars bounce around
        // if (hasCrashed) DespawnAction(this);
        // else Crash();

        if (!hasCrashed) Crash();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // despawn trigger
        if (collision.TryGetComponent(out DespawnTrigger despawnTrigger)) DespawnAction(this);
    }
}