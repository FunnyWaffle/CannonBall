using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class ParticleSpawnExecutor : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionPrefab;

        public void ExecuteExplosionParticlesSpawn(Vector3 position)
        {
            GameObject.Instantiate(_explosionPrefab.gameObject, position, Quaternion.identity);
        }
    }
}
