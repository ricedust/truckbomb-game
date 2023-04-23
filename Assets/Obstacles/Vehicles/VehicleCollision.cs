using System;
using UnityEngine;

public class VehicleCollision : MonoBehaviour
{
    [SerializeField] private PoolableObject poolableObject;
    [SerializeField] private VehicleExploder exploder;

    public event Action OnCollision;
    
    public int collisionCount { get; private set; }
    private void OnEnable() => poolableObject.OnReset += ResetCollisions;
    private void OnDisable() => poolableObject.OnReset -= ResetCollisions;
    private void ResetCollisions() => collisionCount = 0;

    public void ForceCollision() => collisionCount++;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCount++;
        OnCollision?.Invoke();
            
        // explode and despawn vehicle on the second collision
        if (collisionCount > 1)
        {
            EffectsManager.instance.CreateExplosion(transform.position);
            exploder.Explode();
            poolableObject.Despawn();
        }
    }
}