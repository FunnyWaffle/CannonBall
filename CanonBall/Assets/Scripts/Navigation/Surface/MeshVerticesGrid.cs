using Assets.Scripts.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    public class MeshVerticesGrid
    {
        private readonly Dictionary<Vector3Int, List<int>> _triangleGrid = new();

        private readonly float _cellSize;

        public MeshVerticesGrid(Vector3[] vertices, int[] triangles, float cellSize)
        {
            _cellSize = cellSize;

            InitializeCells(vertices, triangles);
        }

        public bool TryGetTriangles(Vector3 position, out IReadOnlyList<int> triangles)
        {
            var cell = WorldToCell(position);

            if (_triangleGrid.TryGetValue(cell, out var localTriangles))
            {
                triangles = localTriangles;

                return true;
            }

            triangles = null;
            return false;
        }

        private void InitializeCells(Vector3[] vertices, int[] triangles)
        {
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int aIndex = triangles[i],
                    bIndex = triangles[i + 1],
                    cIndex = triangles[i + 2];

                var a = vertices[aIndex];
                var b = vertices[bIndex];
                var c = vertices[cIndex];

                var min = Vector3.Min(Vector3.Min(a, b), c);
                var max = Vector3.Max(Vector3.Max(a, b), c);

                var minCell = WorldToCell(min);
                var maxCell = WorldToCell(max);

                AddTriangle(minCell, maxCell, i);
            }
        }

        private Vector3Int WorldToCell(Vector3 coords)
        {
            var scaledCoords = coords / _cellSize;

            var flatX = Mathf.FloorToInt(scaledCoords.x);
            var flatY = Mathf.FloorToInt(scaledCoords.y);
            var flatZ = Mathf.FloorToInt(scaledCoords.z);

            return new Vector3Int(flatX, flatY, flatZ);
        }

        private void AddTriangle(Vector3Int min, Vector3Int max, int triangleIndex)
        {
            for (int x = min.x; x <= max.x; x++)
                for (int y = min.y; y <= max.y; y++)
                    for (int z = min.z; z <= max.z; z++)
                    {
                        var triangles = _triangleGrid.GetOrCreate(new Vector3Int(x, y, z));

                        triangles.Add(triangleIndex);
                    }
        }
    }
}

