using Assets.Scripts.Space;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieTargetSearch
    {
        private readonly List<ISpatialObject> _foundObjects = new();

        private readonly ZombieTarget _zombieTarget;
        private readonly SpatialGrid _spatialGrid;
        private readonly SpatialSearchShape _spatialSearchShape;
        private readonly SpatialObjectsMap _spatialObjectsMap;

        private readonly float _radius;

        private readonly float _findTargetDelay = 1f;
        private float _findTargetTimer;

        public ZombieTargetSearch(
            ZombieTarget zombieTarget,
            SpatialGrid spatialGrid,
            SpatialSearchShape spatialSearchShape,
            SpatialObjectsMap spatialObjectsMap,
            float radius)
        {
            _zombieTarget = zombieTarget;
            _spatialGrid = spatialGrid;
            _spatialSearchShape = spatialSearchShape;
            _spatialObjectsMap = spatialObjectsMap;

            _radius = radius;

            spatialSearchShape.BuldSearchingShape(
                Mathf.CeilToInt(radius / _spatialGrid.CellSize));
        }

        public bool TrySearchTarget(Vector3 center, float searcherRadius)
        {
            if (!_spatialGrid.HasObjects)
                return false;

            if (!HasDelayToSearchPassed()
                || IsCurrentTargetSuitable())
                return true;

            var cellSize = _spatialGrid.CellSize;
            var radiusInCells = Mathf.CeilToInt(_radius / cellSize);

            var centerCell = _spatialGrid.CalculateCell(center);

            for (int r = 0; r < radiusInCells; r++)
            {
                _foundObjects.Clear();

                var offsets = _spatialSearchShape.GetShape(r);

                foreach (var offset in offsets)
                {
                    var cell = centerCell + offset;
                    _spatialGrid.TryGetObjects(cell, _foundObjects);

                }

                if (_foundObjects.Count > 0)
                {
                    FindNearestTarget(center, searcherRadius);
                    _findTargetTimer -= _findTargetDelay;
                    return true;
                }
            }

            return false;
        }

        private void FindNearestTarget(Vector3 center, float searcherRadius)
        {
            var closestTargetPosition = Vector3.zero;
            ISpatialObject target = null;
            var minDistance = float.PositiveInfinity;

            foreach (var @object in _foundObjects)
            {
                if (!_spatialObjectsMap.TryGetHitBox(@object, out var hitbox)
                    || !hitbox.TryGetFreePoisitionAround(searcherRadius, center, out var targetPosition))
                    continue;

                var currentDistance = Vector3.SqrMagnitude(targetPosition - center);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closestTargetPosition = targetPosition;
                    target = @object;
                }
            }

            if (target == null)
                return;

            _zombieTarget.Set(closestTargetPosition);
            _zombieTarget.Set(target);
        }

        private bool IsCurrentTargetSuitable()
        {
            var target = _zombieTarget.Target;
            if (target == null)
                return false;

            if (_zombieTarget.HasMoved)
                return false;

            return true;
        }

        private bool HasDelayToSearchPassed()
        {
            _findTargetTimer += Time.deltaTime;

            if (_findTargetTimer < _findTargetDelay)
                return false;

            return true;
        }
    }
}
