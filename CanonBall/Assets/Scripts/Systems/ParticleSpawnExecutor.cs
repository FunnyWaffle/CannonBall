using Assets.Scripts.Spawn;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Systems
{
    public class ParticleSpawnExecutor : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionPrefab;

        [Inject] private Spawner  _spawner;

        public void ExecuteExplosionParticlesSpawn(Vector3 position)
        {
            _spawner.Spawn(_explosionPrefab.gameObject, position, Quaternion.identity);
        }
    }
}
