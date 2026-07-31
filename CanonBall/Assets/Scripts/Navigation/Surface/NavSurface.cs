using Assets.Scripts.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    [RequireComponent(typeof(MeshFilter))]
    public class NavSurface : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;

        [SerializeField] private float _cellSize = .25f;

        private readonly Dictionary<Vector2Int, List<Vector3>> _nodes = new();

#if UNITY_EDITOR

        private void OnValidate()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        [ContextMenu("Create")]
        private void Create()
        {
            var mesh = _meshFilter.sharedMesh;

            var bounds = mesh.bounds;

            var verticles = mesh.vertices;
            var triangles = mesh.triangles;

            var meshVerticesGrid = new MeshVerticesGrid(verticles, triangles, _cellSize);

            var start = bounds.min;
            var sideLength = bounds.size.x;
            var forwardLength = bounds.size.z;
            for (float currentSideLength = 0; currentSideLength <= sideLength; currentSideLength += _cellSize)
                for (float currentForwardLength = 0; currentForwardLength <= forwardLength; currentForwardLength += _cellSize)
                {
                    var point = start + currentSideLength * Vector3.right + currentForwardLength * Vector3.forward;

                    if (!meshVerticesGrid.TryGetTriangles(point, out var triangleIndexes))
                        break;

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

                        if (IsPointInTriangle(barycentricA, barycentricB, barycentricC))
                            continue;

                        var surfacePoint = GetPointOnSurface(a, b, c, barycentricA, barycentricB, barycentricC);

                        _nodes.Add(surfacePoint.FloorToIntXZ(_cellSize), surfacePoint);
                    }
                }
        }

        private (float, float, float) GetBarycentricWeights(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
        {
            var a2d = new Vector2(a.x, a.z);

            var edgeAB = new Vector2(b.x, b.z) - a2d;
            var edgeAC = new Vector2(c.x, c.z) - a2d;
            var vectorPointA = new Vector2(point.x, point.z) - a2d;

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

#endif
    }
}