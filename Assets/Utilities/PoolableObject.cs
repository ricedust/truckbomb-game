using System;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    /// <summary>OnReset is invoked whenever this object is spawned</summary>
    public event Action OnReset;

    private Action<PoolableObject> DespawnAction;
    public void InitializeDespawnAction(Action<PoolableObject> despawnAction) => DespawnAction = despawnAction;
    
    /// <summary>Deactivate and return object to the pool</summary>
    public void Despawn() => DespawnAction(this);
    public void Reset() => OnReset?.Invoke();
}