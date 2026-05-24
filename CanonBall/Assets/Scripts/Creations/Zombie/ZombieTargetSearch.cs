using Assets.Scripts.Space;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieTargetSearch
    {
        private readonly SpatialGrid _spatialGrid;
        private readonly SpatialSearchShape _spatialSearchShape;

        private readonly List<ISpatialObject> _foundObjects = new();

        private readonly float _radius;

        public ZombieTargetSearch(
            SpatialGrid spatialGrid,
            SpatialSearchShape spatialSearchShape,
            float radius)
        {
            _spatialGrid = spatialGrid;
            _spatialSearchShape = spatialSearchShape;
            _radius = radius;

            spatialSearchShape.BuldSearchingShape(
                Mathf.CeilToInt(radius / _spatialGrid.CellSize));
        }

        public bool HasPossibleTargets => _spatialGrid.HasObjects;

        public bool TryGetTarget(
            Vector3 center,
            out Vector3 target)
        {
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
                    target = FindNearestTarget(center);
                    return true;
                }
            }


            target = Vector3.zero;
            return false;
        }

        private Vector3 FindNearestTarget(Vector3 center)
        {
            var target = Vector3.zero;
            var minDistance = float.PositiveInfinity;

            foreach (var @object in _foundObjects)
            {
                Vector3 objectPosition = @object.Position;
                var currentDistance = Vector3.Distance(center, objectPosition);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    target = objectPosition;
                }
            }

            return target;
        }
    }
}
