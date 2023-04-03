using UnityEngine;

/// <summary>Vehicle is a public facing class for vehicle operations and information</summary>
public class Vehicle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VehicleDriver vehicleDriver;
    [SerializeField] private VehicleLaneChanger laneChanger;
    [SerializeField] private VehicleCollision vehicleCollision;
    [SerializeField] private VehicleCrasher vehicleCrasher;

    public int collisionCount => vehicleCollision.collisionCount;
    public void ApplyForce(Vector2 forceVector) => vehicleCrasher.ApplyForce(forceVector);
    
    /// <summary>Initialize vehicle position with starting lane and the spawn y position</summary>
    public void SetInitialPosition(int lane, float spawnYLevel)
    {
        transform.position = new Vector2(EnvironmentManager.instance.laneXPositions[lane], spawnYLevel);
        laneChanger.SetLane(lane);
    }

    public void SetInitialSpeed(float speed) => vehicleDriver.SetInitialSpeed(speed);
}