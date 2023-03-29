using System.Collections;
using UnityEngine;

public class ObstacleManager : StaticInstance<ObstacleManager>
{
    [SerializeField] private VehicleSpawner vehicleSpawner;
    
    public void StartSpawningVehicles() => vehicleSpawner.StartSpawning();
}