using System;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfBoundsDespawner : MonoBehaviour
{
    [SerializeField] private PoolableObject poolableObject;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out DespawnTrigger despawnTrigger)) poolableObject.Despawn();
    }
}