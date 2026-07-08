using Assets.Scripts.Combat;
using Assets.Scripts.EnemyAttractionObjects;
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
        private readonly EnemyAttractionObject _enemyAttractionObject;
        private readonly World _world;

        private readonly float _radius;

        private readonly float _findTargetDelay = 1f;
        private float _findTargetTimer;

        public ZombieTargetSearch(
            ZombieTarget zombieTarget,
            SpatialGrid spatialGrid,
            SpatialSearchShape spatialSearchShape,
            EnemyAttractionObject enemyAttractionObject,
            World world,
            float radius)
        {
            _zombieTarget = zombieTarget;
            _spatialGrid = spatialGrid;
            _spatialSearchShape = spatialSearchShape;
            _enemyAttractionObject = enemyAttractionObject;
            _world = world;

            _radius = radius;

            spatialSearchShape.BuldSearchingShape(
                Mathf.CeilToInt(radius / _spatialGrid.CellSize));
        }

        public bool TrySearchTarget(Vector3 center, float searcherRadius)
        {
            if (!_spatialGrid.HasObjects)
                return false;

            if (!HasDelayToSearchPassed())
                return true;

            EntityComponents targetComponents = null;
            HitBox targetHitbox = null;
            HitBoxAttackPlaceReservation placeReservation = default;
            Vector3 targetPosition = default;
            float distanceToTarget = float.MaxValue;

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

                if (FindNearestTarget(center, searcherRadius,
                    out targetComponents, out targetHitbox,
                    out placeReservation, out targetPosition,
                    out distanceToTarget))
                {
                    _findTargetTimer = Time.time + _findTargetDelay;
                    break;
                }
            }

            var currentTargetSuitable = _zombieTarget.IsSuitable;
            if (currentTargetSuitable)
            {
                var currentDistance = Vector3.SqrMagnitude(_zombieTarget.AttackPosition - center);

                if (currentDistance < distanceToTarget)
                {
                    distanceToTarget = currentDistance;
                    targetComponents = null;
                    targetHitbox?.ReleaseReservation(placeReservation);
                }
            }

            if (targetComponents == null)
            {
                var hitBox = _enemyAttractionObject.HitBox;

                if (hitBox.TryGetFreePoisitionAround(searcherRadius, center, out var position, out var reservation))
                {
                    var currentDistance = Vector3.SqrMagnitude(position - center);
                    if (currentDistance < distanceToTarget)
                    {
                        targetPosition = position;
                        targetComponents = _enemyAttractionObject.Components;

                        targetHitbox?.ReleaseReservation(placeReservation);

                        targetHitbox = hitBox;
                        placeReservation = reservation;
                    }
                    else if (currentTargetSuitable)
                        return true;
                }
                else if (currentTargetSuitable)
                    return true;
            }

            SetTarget(
                targetComponents,
                targetHitbox,
                placeReservation, targetPosition);

            return false;
        }

        private bool FindNearestTarget(Vector3 center, float searcherRadius,
            out EntityComponents components, out HitBox hitBox,
            out HitBoxAttackPlaceReservation reservation, out Vector3 position,
            out float distanceToTarget)
        {
            hitBox = null;
            components = null;
            reservation = default;
            position = Vector3.zero;

            distanceToTarget = float.MaxValue;

            if (_foundObjects.Count > 0)
                foreach (var @object in _foundObjects)
                {
                    if (!_world.EntityComponents.TryGetValue(@object, out var currentComponents)
                        || _zombieTarget.Compare(currentComponents)
                        || !currentComponents.TryGet<HitBox>(out var currentHitBox)
                        || !currentHitBox.TryGetFreePoisitionAround(searcherRadius, center, out var targetPosition, out var currentReservation))
                        continue;

                    var currentDistance = Vector3.SqrMagnitude(targetPosition - center);
                    if (currentDistance < distanceToTarget)
                    {
                        distanceToTarget = currentDistance;
                        position = targetPosition;
                        components = currentComponents;

                        hitBox?.ReleaseReservation(reservation);

                        hitBox = currentHitBox;
                        reservation = currentReservation;
                    }
                    else
                        currentHitBox?.ReleaseReservation(currentReservation);

                }

            if (components == null)
                return false;

            return true;
        }

        private bool HasDelayToSearchPassed()
        {
            if (Time.timeSinceLevelLoad <= _findTargetTimer)
                return false;

            return true;
        }

        private void SetTarget(EntityComponents entityComponents, HitBox hitBox, HitBoxAttackPlaceReservation reservation, Vector3 position)
        {
            if (_zombieTarget.Compare(entityComponents))
            {
                hitBox?.ReleaseReservation(reservation);

                return;
            }

            _zombieTarget.Set(
            entityComponents,
            hitBox,
            reservation, position);
        }
    }
}
