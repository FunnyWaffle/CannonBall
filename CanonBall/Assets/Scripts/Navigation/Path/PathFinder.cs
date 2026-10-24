using Assets.Scripts.Navigation.Agent;
using Assets.Scripts.Navigation.Surface;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Path
{
    [Serializable]
    public class PathFinder
    {
        private readonly AStar _aStar;
        private readonly SurfaceRepository _surfaceRepository;
        private readonly PathSmoother _pathSmoother;

        public PathFinder(AStar aStar, SurfaceRepository surfaceRepository, PathSmoother pathSmoother)
        {
            _aStar = aStar;
            _surfaceRepository = surfaceRepository;
            _pathSmoother = pathSmoother;
        }

#if UNITY_EDITOR
        public event Action<IReadOnlyList<Vector3>> RawPathCreated;
        public event Action<IReadOnlyList<Vector3>> SmoothedPathCreated;
#endif

        public bool TryCalculateNewPathNonAlloc(Vector3 start, Vector3 destination, NavAgent navAgent, List<Vector3> path)
        {
            path.Clear();

            if (!_surfaceRepository.TryGetSurfaceSequence(start, destination, out var sequence))
                return false;

            if (!_aStar.FindBestPath(sequence, start, destination, navAgent, out var goal))
                return false;

            BuildPath(path, goal);

#if UNITY_EDITOR
            RawPathCreated?.Invoke(new List<Vector3>(path));
#endif

            _pathSmoother.StringPulling(path, .25f, navAgent.StepHeight * 2f, navAgent, sequence);

#if UNITY_EDITOR
            SmoothedPathCreated?.Invoke(path);
#endif

            return true;
        }
        private void BuildPath(List<Vector3> path, PathNode goal)
        {
            var node = goal;
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Parent;
            }

            path.Reverse();
        }
    }
}
