using System;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class WavesExecutor : MonoBehaviour
    {
        [SerializeField] private EnemySpawnZone[] _enemySpawnZones;

        [SerializeField] private int _firstWaveEnemiesCount = 20;

        private int _waveIndex = 0;
        private readonly int _waveEnemiesCountMultiplyer = 2;

        public event Action<int> WaveEnded;

        private void Start()
        {
            var enemiesContainer = new GameObject("Enemies").transform;
            foreach (var zone in _enemySpawnZones)
            {
                zone.AllEnemiesDied += OnEnemiesDied;
                zone.SetEnemiesContainer(enemiesContainer);
            }

            StartNewWave();
        }

        private void OnEnemiesDied()
        {
            foreach (var zone in _enemySpawnZones)
            {
                if (zone.AliveEnemiesCount != 0)
                    return;
            }

            StartNewWave();
        }

        private void StartNewWave()
        {
            WaveEnded?.Invoke(_waveIndex);

            var enemiesCount = _firstWaveEnemiesCount * _waveIndex * _waveEnemiesCountMultiplyer;
            var enemiesPerZone = enemiesCount == 0 ? _firstWaveEnemiesCount : enemiesCount / _enemySpawnZones.Length;

            foreach (var zone in _enemySpawnZones)
            {
                zone.SpawnEnemies(enemiesPerZone);
            }

            _waveIndex++;
        }
    }
}
