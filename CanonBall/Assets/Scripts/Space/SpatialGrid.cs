using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Space
{
    public class SpatialGrid : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 4f;

        private readonly Dictionary<Vector3Int, List<ISpatialObject>> _grid = new();
        private readonly Dictionary<ISpatialObject, Vector3Int> _objectCells = new();
        private readonly Queue<List<ISpatialObject>> _listPool = new();

        public float CellSize => _cellSize;
        public bool HasObjects => _grid.Count > 0;

        public void Add(ISpatialObject spatialObject)
        {
            var cell = CalculateCell(spatialObject.Position);

            var objects = GetObjects(cell);
            objects.Add(spatialObject);

            _objectCells[spatialObject] = cell;

            spatialObject.PositionChanged += UpdateCell;
        }

        public bool TryGetObjects(Vector3Int cell, List<ISpatialObject> objects)
        {
            if (_grid.TryGetValue(cell, out var spatialObjects))
            {
                objects.AddRange(spatialObjects);
                return true;
            }

            return false;
        }

        public void Remove(ISpatialObject spatialObject)
        {
            var cell = _objectCells[spatialObject];

            RemoveFromObjects(cell, spatialObject);

            _objectCells.Remove(spatialObject);

            spatialObject.PositionChanged -= UpdateCell;
        }

        public Vector3Int CalculateCell(Vector3 position)
        {
            var scaledPosition = position / _cellSize;
            return new Vector3Int(
                Mathf.FloorToInt(scaledPosition.x),
                Mathf.FloorToInt(scaledPosition.y),
                Mathf.FloorToInt(scaledPosition.z));
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
