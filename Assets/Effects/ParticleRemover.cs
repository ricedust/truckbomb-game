using UnityEngine;

namespace Effects
{
    public class ParticleRemover : MonoBehaviour
    {
        [SerializeField] private PoolableObject poolableObject;
        private void OnParticleSystemStopped()
        {
            poolableObject.Despawn();
        }
    }
}