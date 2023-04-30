using System;
using UnityEngine;

public class VehicleExploder : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private int maxAffectedVehicles;

    public static event Action<Vector2> OnExplosion;
    
    /// <summary>Apply outward force to all other vehicles in explosion radius</summary>
    public void Explode()
    {
        Vector2 currentPosition = transform.position;
        OnExplosion.Invoke(currentPosition);
        
        // get colliders for all vehicles in radius
        Collider2D[] colliders = new Collider2D[maxAffectedVehicles];
        Physics2D.OverlapCircleNonAlloc(currentPosition, explosionRadius, colliders);
        
        foreach (Collider2D collider2d in colliders)
        {
            // skip over if
            if (!collider2d // element is null
                || !collider2d.TryGetComponent(out Vehicle vehicle) // not a vehicle
                || vehicle.GetComponent<VehicleExploder>() == this) // self reference
                continue;
            
            // calculate force angle
            Vector2 vehiclePosition = vehicle.transform.position;
            Vector2 explosionAngle = (vehiclePosition - currentPosition).normalized;
            
            Debug.DrawLine(currentPosition, vehiclePosition, Color.red);

            // decrease explosion force further away
            float explosionForceOverDistance = explosionForce / Vector2.Distance(vehiclePosition, currentPosition);

            vehicle.GetComponent<VehicleCollision>().ForceCollision(); // disables vehicle autopilot
            vehicle.ApplyForce(explosionAngle * explosionForceOverDistance);
        }
    }
}