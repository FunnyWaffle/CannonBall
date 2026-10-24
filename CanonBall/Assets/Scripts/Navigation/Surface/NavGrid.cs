using Assets.Scripts.Navigation.Surface.GraphBuilders;
using Assets.Scripts.Spawn.Pools;
using Assets.Scripts.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    public class NavGrid : MonoBehaviour
    {
        [SerializeField] private float _cellSize = .25f;

        [SerializeField] private SerializableDictionary<Vector3Int, Vector3> _nodes;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Terrain _terrain;

        private readonly SimplePool<List<Vector3>> _nodesCollectionsPool = new();

        private ISurfaceGraphBuilder _surfaceGraphBuilder;

        private Vector3 _localCellStep;

        public Vector3 Min
        {
            get
            {
                if (_meshRenderer != null)
                    return _meshRenderer.bounds.min;
                if (_terrain != null)
                    return transform.TransformPoint(_terrain.terrainData.bounds.min);

                return default;
            }
        }
        public Vector3 Max
        {
            get
            {
                if (_meshRenderer != null)
                    return _meshRenderer.bounds.max;
                if (_terrain != null)
                    return transform.TransformPoint(_terrain.terrainData.bounds.max);

                return default;
            }
        }

        public Vector3 Size
        {
            get
            {
                if (_meshRenderer != null)
                    return _meshRenderer.bounds.size;
                if (_terrain != null)
                    return _terrain.terrainData.size;

                return default;
            }
        }

        public bool TryGetNode(Vector3 position, out Vector3 node)
        {
            var cell = CalculateLocalCell(position);

            if (!_nodes.TryGetValue(cell, out var localNode))
            {
                node = default;
                return false;
            }

            node = transform.TransformPoint(localNode);

            return true;
        }

        public bool TryGetNeighbours(Vector3 position, out IReadOnlyList<Vector3> nodes)
        {
            var cell = CalculateLocalCell(position);

            var mutableNodes = _nodesCollectionsPool.Get();

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                    {
                        var neigbourCell = new Vector3Int(
                            cell.x + x,
                            cell.y + y,
                            cell.z + z);

                        if (!_nodes.TryGetValue(neigbourCell, out var neigbour))
                            continue;

                        neigbour = transform.TransformPoint(neigbour);

                        mutableNodes.Add(neigbour);
                    }

            nodes = mutableNodes;

            return nodes.Count > 0;
        }

        public void Return(IReadOnlyCollection<Vector3> nodes)
        {
            var typedNodes = (List<Vector3>)nodes;

            typedNodes.Clear();

            _nodesCollectionsPool.Return(typedNodes);
        }

        public bool Contains(Vector3 position)
        {
            if (position.IsBigger(Min)
                && position.IsLess(Max))
                return true;

            return false;
        }

        public float SampleHeight(Vector3 worldPoint)
        {
            if (_terrain != null)
            {
                return _terrain.transform.position.y + _terrain.SampleHeight(worldPoint);
            }
            if (_meshRenderer != null)
            {
                return worldPoint.y;
            }

            return worldPoint.y;
        }

        private Vector3Int CalculateLocalCell(Vector3 worldPosition)
        {
            var localPosition = transform.InverseTransformPoint(worldPosition);

            var cellStep = CalculateCellStep();

            if (cellStep != _localCellStep)
                Create();

            return localPosition.FloorToInt(_localCellStep);
        }

        private Vector3 CalculateCellStep() => new(
                transform.InverseTransformVector(Vector3.right * _cellSize).magnitude,
                        transform.InverseTransformVector(Vector3.up * _cellSize).magnitude,
                        transform.InverseTransformVector(Vector3.forward * _cellSize).magnitude);

        [ContextMenu("Create")]
        private void Create()
        {
            _localCellStep = CalculateCellStep();

            _nodes.Clear();

            _ = _surfaceGraphBuilder.CreateAsyncNonAlloc(_localCellStep, _nodes);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (TryGetComponent(out _meshFilter))
            {
                _surfaceGraphBuilder = new MeshFilterBuilder(_meshFilter);
            }
            else if (TryGetComponent(out _terrain))
            {
                _surfaceGraphBuilder = new TerrainBuilder(_terrain);
            }

            TryGetComponent(out _meshRenderer);
        }

#endif
    }
}