using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float vehicleCollisionForce;

    public static event Action OnVehicleCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Vehicle vehicle))
        {
            PushAway(vehicle);
            OnVehicleCollision?.Invoke();
        }
    }

    private void PushAway(Vehicle vehicle)
    {
        Vector2 angleOfCollision = (vehicle.transform.position - transform.position);
        angleOfCollision.Normalize();

        vehicle.ApplyForce(angleOfCollision * vehicleCollisionForce);
    }
}
