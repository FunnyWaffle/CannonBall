using Assets.Scripts.Wrappers;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Navigation.Obstacles
{
    public class NavObstacleMap : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 1f;

        private readonly Dictionary<Vector3Int, HashSet<NavObstacle>> _navObstacles = new();

        [Inject]
        public void Initialize(params NavObstacle[] obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                var bounds = obstacle.Bounds;
                var min = bounds.min;
                var max = bounds.max;

                var minCell = min.FloorToInt(_cellSize);
                var maxCell = max.FloorToInt(_cellSize);

                for (int x = minCell.x; x < maxCell.x; x++)
                    for (int y = minCell.y; y < maxCell.y; y++)
                        for (int z = minCell.z; z < maxCell.z; z++)
                        {
                            _navObstacles.Add(new Vector3Int(x, y, z), obstacle);
                        }
            }
        }

        public bool TryGetNavOstacles(Vector3 position, out IEnumerable<NavObstacle> obstacles)
        {
            obstacles = null;

            var cell = position.FloorToInt(_cellSize);

            if (!_navObstacles.TryGetValue(cell, out var localObstacles))
            {
                obstacles = localObstacles;
                return false;
            }

            return obstacles != null;
        }
    }
}
