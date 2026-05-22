using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creations
{
    public class SpatialGrid : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 4f;

        private readonly Dictionary<Vector3Int, List<ISpatialObject>> _grid = new();
        private readonly Dictionary<ISpatialObject, Vector3Int> _objectCells = new();
        private readonly Queue<List<ISpatialObject>> _listPool = new();

        public void Add(ISpatialObject spatialObject)
        {
            var cell = CalculateCell(spatialObject.Position);

            var objects = GetObjects(cell);
            objects.Add(spatialObject);

            _objectCells[spatialObject] = cell;

            spatialObject.PositionChanged += UpdateCell;
        }

        public void Remove(ISpatialObject spatialObject)
        {
            var cell = _objectCells[spatialObject];

            RemoveFromObjects(cell, spatialObject);

            _objectCells.Remove(spatialObject);

            spatialObject.PositionChanged -= UpdateCell;
        }

        public bool TryGetObjects(
            Vector3 center,
            float radius,
            List<ISpatialObject> objects)
        {
            var centerCell = CalculateCell(center);

            var radiusInCells = Mathf.CeilToInt(radius / _cellSize);

            for (int r = 0; r < radiusInCells; r++)
            {
                var found = false;

                for (int x = -r; x <= r; x++)
                    for (int y = -r; y <= r; y++)
                        for (int z = -r; z <= r; z++)
                        {
                            if (Mathf.Max(
                                Mathf.Abs(x),
                                Mathf.Abs(y),
                                Mathf.Abs(z)) != r)
                            {
                                continue;
                            }

                            var cell = new Vector3Int(
                                centerCell.x + x,
                                centerCell.y + y,
                                centerCell.z + z);

                            if (_grid.TryGetValue(cell, out var privateObjects))
                            {
                                if (privateObjects.Count > 0)
                                {
                                    objects.AddRange(privateObjects);
                                    found = true;
                                }
                            }
                        }

                if (found)
                    return true;
            }
            return false;
        }

        private void UpdateCell(object sender, Vector3 position)
        {
            var spatialObject = sender as ISpatialObject;

            var cell = CalculateCell(spatialObject.Position);
            var oldCell = _objectCells[spatialObject];

            if (cell == oldCell)
                return;

            RemoveFromObjects(oldCell, spatialObject);

            var objects = GetObjects(cell);
            objects.Add(spatialObject);

            _objectCells[spatialObject] = cell;
        }

        private List<ISpatialObject> GetObjects(Vector3Int cell)
        {
            if (!_grid.TryGetValue(cell, out var objects))
            {
                if (_listPool.Count > 0)
                    objects = _listPool.Dequeue();
                else
                    objects = new List<ISpatialObject>();

                _grid[cell] = objects;
            }

            return objects;
        }

        private Vector3Int CalculateCell(Vector3 position)
        {
            var scaledPosition = position / _cellSize;
            return new Vector3Int(
                Mathf.FloorToInt(scaledPosition.x),
                Mathf.FloorToInt(scaledPosition.y),
                Mathf.FloorToInt(scaledPosition.z));
        }

        private void RemoveFromObjects(Vector3Int cell, ISpatialObject spatialObject)
        {
            var objects = GetObjects(cell);
            objects.Remove(spatialObject);

            if (objects.Count == 0)
            {
                _grid.Remove(cell);
                _listPool.Enqueue(objects);
            }
        }
    }

    public interface ISpatialObject
    {
        public Vector3 Position { get; }

        public event EventHandler<Vector3> PositionChanged;
    }
}
