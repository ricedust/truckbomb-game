using UnityEngine;

public class EffectsManager : StaticInstance<EffectsManager>
{
    [SerializeField] private ObjectPooler effectsPool;

    private void OnEnable()
    {
        VehicleExploder.OnExplosion += CreateExplosion;
    }

    private void OnDisable()
    {
        VehicleExploder.OnExplosion -= CreateExplosion;
    }

    private void CreateExplosion(Vector2 explosionOrigin)
    {
        ParticleSystem explosionEffect = effectsPool.Spawn().GetComponent<ParticleSystem>();
        explosionEffect.transform.position = explosionOrigin;
        explosionEffect.Play();
    }
}