using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float vehicleCollisionForce;

    public static event Action OnPlayerDamaged;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Vehicle vehicle))
        {
            PushAway(vehicle);
            
            // player should be immune if vehicle has already been hit or player has powerup
            if (vehicle.collisionCount > 1 || PowerupManager.instance.isPowerupActive) return;
            
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
