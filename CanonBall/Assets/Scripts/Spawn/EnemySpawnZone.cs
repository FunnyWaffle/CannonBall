using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Explosion;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Spawn
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [SerializeField] private Vector3 _size;
        [SerializeField] private ZombieCore _enemyPrefab;

        [Inject] private Spawner _spawner;
        [Inject] private ExplosionHandler _explosionHandler;

        private readonly List<ZombieCore> _enemies = new();

        private Transform _enemiesContainer;

        public int AliveEnemiesCount => _enemies.Count;

        public event Action AllEnemiesDied;

        public void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = GetSpawnPosition();

                var enemyCore = _spawner.Spawn(_enemyPrefab, position, Quaternion.identity, _enemiesContainer,
                    _enemyPrefab.name + $"({i})");
                enemyCore.Enable();

                enemyCore.Died += OnEnemyDied;

                var hitbox = enemyCore.Hitbox;

                _enemies.Add(enemyCore);

                _explosionHandler.AddExplosionReceiver(hitbox.Collider, hitbox);
            }
        }

        public void SetEnemiesContainer(Transform container)
        {
            _enemiesContainer = container;
        }

        private Vector3 GetSpawnPosition()
        {
            var halfSize = _size / 2;

            var localPosition = new Vector3(
                Random.Range(-halfSize.x, halfSize.x),
                Random.Range(-halfSize.y, halfSize.y),
                Random.Range(-halfSize.z, halfSize.z)
                );
            var worldPosition = transform.TransformPoint(localPosition);
            return worldPosition;
        }

        private void OnEnemyDied(object enemy, System.EventArgs args)
        {
            if (enemy is ZombieCore zombieCore)
            {
                _enemies.Remove(zombieCore);
                zombieCore.Died -= OnEnemyDied;

                if (_enemies.Count == 0)
                    AllEnemiesDied?.Invoke();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.beige;
            var oldMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _size);
            Gizmos.matrix = oldMatrix;
        }
    }
}
