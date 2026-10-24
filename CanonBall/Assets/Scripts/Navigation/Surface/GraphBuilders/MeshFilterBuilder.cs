using Assets.Scripts.Wrappers;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface.GraphBuilders
{
    public class MeshFilterBuilder : ISurfaceGraphBuilder
    {
        private readonly MeshFilter _meshFilter;

        public MeshFilterBuilder(MeshFilter meshFilter)
        {
            _meshFilter = meshFilter;
        }

        public async Task CreateAsyncNonAlloc(Vector3 cellStep,
            SerializableDictionary<Vector3Int, Vector3> graph)
        {
            var mesh = _meshFilter.sharedMesh;

            var bounds = mesh.bounds;

            var verticles = mesh.vertices;
            var triangles = mesh.triangles;

            var meshVerticesGrid = new MeshVerticesGrid(verticles, triangles, cellStep);

            var start = bounds.min;
            var maxCell = bounds.size.FloorToInt(cellStep);

            for (int x = 0; x <= maxCell.x; x++)
                for (int y = 0; y <= maxCell.y; y++)
                    for (int z = 0; z <= maxCell.z; z++)
                    {
                        var point = start + new Vector3(
                            x * cellStep.x,
                            y * cellStep.y,
                            z * cellStep.z);

                        if (!meshVerticesGrid.TryGetTriangles(point, out var triangleIndexes))
                            continue;

                        var trianglesCount = triangleIndexes.Count;
                        for (int i = 0; i < trianglesCount; i++)
                        {
                            int triangleStart = triangleIndexes[i];

                            int aIndex = triangles[triangleStart];
                            int bIndex = triangles[triangleStart + 1];
                            int cIndex = triangles[triangleStart + 2];

                            var a = verticles[aIndex];
                            var b = verticles[bIndex];
                            var c = verticles[cIndex];

                            var (barycentricA, barycentricB, barycentricC) = GetBarycentricWeights(point, a, b, c);

                            if (!IsPointInTriangle(barycentricA, barycentricB, barycentricC))
                                continue;

                            var surfacePoint = GetPointOnSurface(a, b, c, barycentricA, barycentricB, barycentricC);

                            graph.Add(surfacePoint.FloorToInt(cellStep), surfacePoint);

                            break;
                        }
                    }
        }

        private (float, float, float) GetBarycentricWeights(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
        {
            var a2d = a.ToXZ();

            var edgeAB = b.ToXZ() - a2d;
            var edgeAC = c.ToXZ() - a2d;
            var vectorPointA = point.ToXZ() - a2d;

            var edgeABLengthSq = Vector2.Dot(edgeAB, edgeAB);
            var edgeACLengthSq = Vector2.Dot(edgeAC, edgeAC);
            var edgeDot = Vector2.Dot(edgeAB, edgeAC);

            var pointDotAB = Vector2.Dot(vectorPointA, edgeAB);
            var pointDotAC = Vector2.Dot(vectorPointA, edgeAC);

            var denom = edgeABLengthSq * edgeACLengthSq - edgeDot * edgeDot;

            var barycentricB = (edgeACLengthSq * pointDotAB - edgeDot * pointDotAC) / denom;
            var barycentricC = (edgeABLengthSq * pointDotAC - edgeDot * pointDotAB) / denom;
            var barycentricA = 1 - barycentricB - barycentricC;

            return (barycentricA, barycentricB, barycentricC);
        }

        private bool IsPointInTriangle(float barycentricA, float barycentricB, float barycentricC)
        {
            return barycentricC >= 0
                && barycentricA >= 0
                && barycentricB >= 0;
        }

        private Vector3 GetPointOnSurface(Vector3 a, Vector3 b, Vector3 c,
            float barycentricA, float barycentricB, float barycentricC)
        {
            return a * barycentricA
                + b * barycentricB
                + c * barycentricC;
        }
    }
}
