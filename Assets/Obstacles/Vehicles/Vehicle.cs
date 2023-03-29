using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour, IPoolable
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
    [Header("Explosion")]
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private Action<GameObject> Despawn;

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

    public void InitializeDespawnAction(Action<GameObject> Despawn) => this.Despawn = Despawn;

    public void ApplyForce(Vector2 forceVector)
    {
        rigidbody2d.AddForce(forceVector, ForceMode2D.Impulse);
    }

    private void Drive()
    {
        float gameSpeed = GameManager.instance.gameSpeed;
        rigidbody2d.velocity = (speedRelativeToGameSpeed - gameSpeed) * Vector2.up;
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
        rigidbody2d.AddForce(brakingForce * Time.deltaTime * Vector2.down);

        // clamp downward velocity to the game speed
        if (rigidbody2d.velocity.y < -gameSpeed)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -gameSpeed);
        }
    }
    private void ApplyPanicSteering()
    {
        // noise value between -1 and 1
        float noise = Mathf.PerlinNoise(Time.time, 0) * (Random.value * 2 - 1);

        rigidbody2d.AddTorque(noise * panicSteeringTorqueMultiplier * Time.deltaTime);
        rigidbody2d.AddRelativeForce(panicSteeringForce * Time.deltaTime * Vector2.up);
    }

    private void Explode()
    {
        Vector2 currentPosition = transform.position;
        foreach (Collider2D collision in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
        {
            if (collision.TryGetComponent(out Vehicle vehicle) && vehicle != this)
            {
                Vector2 vehiclePosition = vehicle.transform.position;
                Vector2 explosionAngle = vehiclePosition - currentPosition;

                float explosionForceOverDistance = explosionForce / Vector2.Distance(vehiclePosition, currentPosition);
                vehicle.ApplyForce(explosionAngle * explosionForceOverDistance);
            }
        }
    }

    public void ResetState()
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
        // This is the logic for despawning vehicles after two collisions
        if (hasCrashed)
        {
            Explode();
            Despawn(gameObject);
        }
        else Crash();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // despawn trigger
        if (collision.TryGetComponent(out DespawnTrigger despawnTrigger)) Despawn(gameObject);
    }
}