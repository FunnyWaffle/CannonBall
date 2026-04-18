using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public class WavesExecutor : MonoBehaviour
    {
        [SerializeField] private EnemySpawnZone[] _enemySpawnZones;

        [SerializeField] private int _firstWaveEnemiesCount = 20;

        private int _waveIndex = 1;
        private readonly int _waveEnemiesCountMultiplyer = 2;


        private void Start()
        {
            foreach (var zone in _enemySpawnZones)
            {
                zone.AllEnemiesDied += OnEnemiesDied;
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
            var enemiesCount = _firstWaveEnemiesCount * _waveIndex * _waveEnemiesCountMultiplyer;
            var enemiesPerZone = enemiesCount / _enemySpawnZones.Length;

            foreach (var zone in _enemySpawnZones)
            {
                zone.SpawnEnemies(enemiesPerZone);
            }

            _waveIndex++;
        }
    }
}
