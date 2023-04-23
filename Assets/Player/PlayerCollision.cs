using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerInvincibility playerInvincibility;
    [SerializeField] private float vehicleCollisionForce;

    public static event Action OnPlayerDamaged;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Vehicle vehicle))
        {
            PushAway(vehicle);
            
            // player should not take damage if the vehicle has already been hit
            // or if the player is in an invincible state
            if (vehicle.collisionCount > 1
                || playerInvincibility.IsInvincible()) return;
            OnPlayerDamaged?.Invoke();
        }
    }

    private void PushAway(Vehicle vehicle)
    {
        Vector2 angleOfCollision = (vehicle.transform.position - transform.position);
        angleOfCollision.Normalize();

        vehicle.ApplyForce(angleOfCollision * vehicleCollisionForce);
    }
}