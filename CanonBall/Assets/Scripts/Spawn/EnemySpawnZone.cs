using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Explosion;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [SerializeField] private Vector3 _size;
        [SerializeField] private int _enemiesCount;
        [SerializeField] private ZombieCore _enemyPrefab;

        [Inject] private Spawner _spawner;
        [Inject] private ExplosionHandler _explosionHandler;

        private readonly List<ZombieCore> _enemies = new();

        private Vector3 _startSpawnPosition;
        private Vector3 _endSpawnPosition;

        private void Awake()
        {
            _startSpawnPosition = transform.position + _size / 2;
            _endSpawnPosition = transform.position - _size / 2;
        }

        private void Update()
        {
            int currentEnemyCount = _enemies.Count;
            if (currentEnemyCount < _enemiesCount)
            {
                for (int i = 0; i < _enemiesCount - currentEnemyCount; i++)
                {
                    var position = new Vector3(
                        Random.Range(_startSpawnPosition.x, _endSpawnPosition.x),
                        Random.Range(_startSpawnPosition.y, _endSpawnPosition.y),
                        Random.Range(_startSpawnPosition.z, _endSpawnPosition.z)
                        );
                    var enemyCore = _spawner.Spawn(_enemyPrefab, position, Quaternion.identity);
                    enemyCore.Died += OnEnemyDied;

                    var hitbox = enemyCore.Hitbox;

                    _enemies.Add(enemyCore);

                    _explosionHandler.AddExplosionReceiver(hitbox.Collider, hitbox);
                }
            }
        }

        private void OnEnemyDied(object enemy, System.EventArgs args)
        {
            if (enemy is ZombieCore zombieCore)
            {
                _enemies.Remove(zombieCore);
                zombieCore.Died -= OnEnemyDied;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.beige;
            Gizmos.DrawWireCube(transform.position, _size);
        }
    }
}
