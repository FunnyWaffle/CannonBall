using Assets.Scripts.Spawn.Pools;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Navigation.Surface
{
    public class SurfaceSequence
    {
        private readonly SimplePool<HashSet<Vector3>> _nodesCollectionsPool = new();
        private readonly IReadOnlyCollection<NavGrid> _surfaces;

        public SurfaceSequence(IReadOnlyCollection<NavGrid> surfaces)
        {
            _surfaces = surfaces;
        }

        public bool GetNodesIn(Vector3 position, out IReadOnlyCollection<Vector3> nodes)
        {
            var found = false;

            var mutableNodes = _nodesCollectionsPool.Get();

            foreach (var surface in _surfaces)
            {
                if (surface.TryGetNode(position, out var tempNode))
                {
                    found = true;
                    mutableNodes.Add(tempNode);
                }
            }

            if (!found)
            {
                nodes = null;

                _nodesCollectionsPool.Return(mutableNodes);

                return false;
            }

            nodes = mutableNodes;

            return true;
        }

        public bool GetNeigbourNodes(Vector3 node, out IReadOnlyCollection<Vector3> nodes)
        {
            var mutableNodes = _nodesCollectionsPool.Get();

            var found = false;

            foreach (var surface in _surfaces)
            {
                if (surface.TryGetNeighbours(node, out var tempNodes))
                {
                    found = true;
                    mutableNodes.AddRange(tempNodes);
                }

                surface.Return(tempNodes);
            }

            if (!found)
            {
                nodes = null;

                _nodesCollectionsPool.Return(mutableNodes);

                return false;
            }

            nodes = mutableNodes;

            return true;
        }

        public void Return(IReadOnlyCollection<Vector3> nodes)
        {
            var typedNodes = (HashSet<Vector3>)nodes;

            typedNodes.Clear();

            _nodesCollectionsPool.Return(typedNodes);
        }

        public float SampleHeight(Vector3 point)
        {
            foreach (var surface in _surfaces)
            {
                if (!surface.Contains(point))
                    continue;

                return surface.SampleHeight(point);
            }

            return point.y;
        }
    }
}
