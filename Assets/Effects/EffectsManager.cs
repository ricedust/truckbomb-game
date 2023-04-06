using Unity.VisualScripting;
using UnityEngine;

public class EffectsManager : StaticInstance<EffectsManager>
{
    [SerializeField] private ObjectPooler effectsPool;
    
    public void CreateExplosion(Vector2 position)
    {
        ParticleSystem explosionEffect = effectsPool.Spawn().GetComponent<ParticleSystem>();
        explosionEffect.transform.position = position;
        explosionEffect.Play();
        
    }
}