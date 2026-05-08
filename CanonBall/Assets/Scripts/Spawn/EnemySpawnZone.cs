using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Explosion;
using Assets.Scripts.Shop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Spawn
{
    public class EnemySpawnZone : MonoBehaviour
    {
        [SerializeField] private Vector3 _size;
        [SerializeField] private ZombieView _enemyPrefab;

        [Inject] private Spawner<ZombieController> _spawner;
        [Inject] private ExplosionHandler _explosionHandler;

        private readonly List<ZombieController> _enemies = new();

        private Transform _enemiesContainer;

        public int AliveEnemiesCount => _enemies.Count;

        public event Action AllEnemiesDied;

        public async Task SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = GetSpawnPosition();

                var enemyCore = await _spawner.Spawn(ItemTypes.Zombie, position, Quaternion.identity, _enemiesContainer);

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

        private void OnEnemyDied(object enemy, EventArgs args)
        {
            if (enemy is ZombieController zombieCore)
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
