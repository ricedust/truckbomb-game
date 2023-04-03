using UnityEngine;

public class VehicleExploder : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    /// <summary>Apply outward force to all other vehicles in explosion radius</summary>
    public void Explode()
    {
        Vector2 currentPosition = transform.position;
        foreach (Collider2D collision in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
        {
            if (collision.TryGetComponent(out Vehicle vehicle) && vehicle.GetComponent<VehicleExploder>() != this)
            {
                Vector2 vehiclePosition = vehicle.transform.position;
                Vector2 explosionAngle = vehiclePosition - currentPosition;

                float explosionForceOverDistance = explosionForce / Vector2.Distance(vehiclePosition, currentPosition);
                vehicle.ApplyForce(explosionAngle * explosionForceOverDistance);
            }
        }
    }
}