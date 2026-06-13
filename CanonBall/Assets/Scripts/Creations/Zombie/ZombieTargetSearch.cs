using Assets.Scripts.Combat;
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
        private readonly World _world;

        private readonly float _radius;

        private readonly float _findTargetDelay = 1f;
        private float _findTargetTimer;

        public ZombieTargetSearch(
            ZombieTarget zombieTarget,
            SpatialGrid spatialGrid,
            SpatialSearchShape spatialSearchShape,
            World world,
            float radius)
        {
            _zombieTarget = zombieTarget;
            _spatialGrid = spatialGrid;
            _spatialSearchShape = spatialSearchShape;
            _world = world;

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
                    _findTargetTimer = Time.time + _findTargetDelay;
                    return true;
                }
            }

            return false;
        }

        private void FindNearestTarget(Vector3 center, float searcherRadius)
        {
            var closestTargetPosition = Vector3.zero;
            ISpatialObject bestTarget = null;
            HitBox bestHitBox = null;
            EntityComponents bestTargetComponents = null;
            var minDistance = float.MaxValue;
            HitBoxAttackPlaceReservation bestReservation = default;

            foreach (var @object in _foundObjects)
            {
                if (!_world.EntityComponents.TryGetValue(@object, out var components)
                    || !components.TryGet<HitBox>(out var hitBox)
                    || !hitBox.TryGetFreePoisitionAround(searcherRadius, center, out var targetPosition, out var reservation))
                    continue;

                var currentDistance = Vector3.SqrMagnitude(targetPosition - center);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closestTargetPosition = targetPosition;
                    bestReservation = reservation;
                    bestTarget = @object;
                    bestHitBox = hitBox;
                    bestTargetComponents = components;
                }
            }

            if (bestTarget == null)
                return;

            _zombieTarget.Set(
                bestTargetComponents,
                bestHitBox,
                bestReservation, closestTargetPosition);
        }

        private bool IsCurrentTargetSuitable()
        {
            if (!_zombieTarget.IsSuitable)
                return false;

            return true;
        }

        private bool HasDelayToSearchPassed()
        {
            if (Time.timeSinceLevelLoad <= _findTargetTimer)
                return false;

            return true;
        }
    }
}
