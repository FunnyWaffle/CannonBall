using Assets.Scripts.Wrappers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    [Serializable]
    public class SurfaceRepository
    {
        [SerializeField] private NavGrid[] _surfaces;
        [SerializeField] private float _cellSizeThreshold = 1f;

        private readonly Dictionary<Vector3Int, List<NavGrid>> _surfaceGrid = new();

        private float _cellSize;

        public void Initialize()
        {
            if (_surfaceGrid.Count > 0)
                return;

            CalculateCellSize();
            Register();
        }

        public bool TryGetSurface(Vector3 position, out IReadOnlyList<NavGrid> surfaces)
        {
            var cell = position.FloorToInt(_cellSize);

            var found = _surfaceGrid.TryGetValue(cell, out var result);

            surfaces = result;
            return found;
        }

        public bool TryGetSurfaceSequence(Vector3 from, Vector3 to, out SurfaceSequence surfaces)
        {
            var startCell = from.FloorToInt(_cellSize);
            var endCell = to.FloorToInt(_cellSize);

            var sizeX = Mathf.Abs(startCell.x - endCell.x);
            var sizeY = Mathf.Abs(startCell.y - endCell.y);
            var sizeZ = Mathf.Abs(startCell.z - endCell.z);

            var hashSet = new HashSet<NavGrid>();

            for (int x = 0; x <= sizeX; x++)
                for (int y = 0; y <= sizeY; y++)
                    for (int z = 0; z <= sizeZ; z++)
                    {
                        var cell = new Vector3Int(
                            startCell.x + x,
                            startCell.y + y,
                            startCell.z + z);

                        if (_surfaceGrid.TryGetValue(cell, out var surfacesInCell))
                            hashSet.UnionWith(surfacesInCell);
                    }

            surfaces = new SurfaceSequence(hashSet);
            return hashSet.Count > 0;
        }

        private void CalculateCellSize()
        {
            _cellSize = float.MaxValue;

            foreach (var surface in _surfaces)
            {
                var size = surface.Size;

                if (size.x >= _cellSizeThreshold)
                    _cellSize = Mathf.Min(_cellSize, size.x);

                if (size.y >= _cellSizeThreshold)
                    _cellSize = Mathf.Min(_cellSize, size.y);

                if (size.z >= _cellSizeThreshold)
                    _cellSize = Mathf.Min(_cellSize, size.z);
            }

            if (_cellSize == float.MaxValue)
                _cellSize = _cellSizeThreshold;
        }

        private void Register()
        {
            foreach (var surface in _surfaces)
            {
                var min = surface.Min;
                var max = surface.Max;

                var minCell = min.FloorToInt(_cellSize);
                var maxCell = max.FloorToInt(_cellSize);

                var sizeX = Mathf.Max(1, maxCell.x - minCell.x);
                var sizeY = Mathf.Max(1, maxCell.y - minCell.y);
                var sizeZ = Mathf.Max(1, maxCell.z - minCell.z);

                for (int x = 0; x <= sizeX; x++)
                    for (int y = 0; y <= sizeY; y++)
                        for (int z = 0; z <= sizeZ; z++)
                        {
                            var cell = new Vector3Int(
                                minCell.x + x,
                                minCell.y + y,
                                minCell.z + z);

                            var cellSurfaces = _surfaceGrid.GetOrCreate(cell);

                            cellSurfaces.Add(surface);
                        }
            }
        }
    }
}
