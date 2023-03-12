using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car : MonoBehaviour, IPoolable
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

    private Action<GameObject> OnDespawn;

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

    public void InitializeDespawnAction(Action<GameObject> DespawnAction)
    {
        OnDespawn = DespawnAction;
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
        // noise value between -1 and 1
        float noise = Mathf.PerlinNoise(Time.time, 0) * (Random.value * 2 - 1);

        rigidbody2d.AddTorque(noise * panicSteeringTorqueMultiplier * Time.deltaTime);
        rigidbody2d.AddRelativeForce(Vector2.up * panicSteeringForce * Time.deltaTime);
    }

    private void Explode()
    {
        foreach (Collider2D collision in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
        {
            if (collision.TryGetComponent(out Car car) && car != this)
            {
                Vector2 explosionAngle = (car.transform.position - transform.position);

                float explosionForceOverDistance = explosionForce / Vector2.Distance(car.transform.position, transform.position);
                car.ApplyForce(explosionAngle * explosionForceOverDistance);
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
        // This is the logic for despawning cars after two collisions
        if (hasCrashed)
        {
            Explode();
            OnDespawn(gameObject);
        }
        else Crash();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // despawn trigger
        if (collision.TryGetComponent(out DespawnTrigger despawnTrigger)) OnDespawn(gameObject);
    }
}