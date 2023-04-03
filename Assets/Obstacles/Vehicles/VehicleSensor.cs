using UnityEngine;
using UnityEngine.Serialization;

class VehicleSensor : MonoBehaviour
{
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private float forwardSensorLength;
    [SerializeField] private float minDistanceToVehicleInFront;

    public bool IsVehicleInFrontTooClose(out float distanceToFrontVehicle)
    {
        distanceToFrontVehicle = float.MaxValue;
        
        // try to find a vehicle in front
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, forwardSensorLength);
        Debug.DrawRay(transform.position, Vector2.up * forwardSensorLength);

        if (hit.collider)
        {
            // check the distance between this vehicle and the vehicle in front
            distanceToFrontVehicle = collider2d.Distance(hit.collider).distance;
            bool isTooClose = distanceToFrontVehicle < minDistanceToVehicleInFront;
            
            // red debug ray if too close
            if (isTooClose) Debug.DrawRay(transform.position, Vector2.up * forwardSensorLength, Color.red);

            return isTooClose;
        }
        return false; // forward sensor didn't detect anything
    }
}