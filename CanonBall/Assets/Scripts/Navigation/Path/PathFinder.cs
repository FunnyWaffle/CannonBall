using Assets.Scripts.Navigation.Obstacles;
using Assets.Scripts.Navigation.Surface;
using Assets.Scripts.Systems;
using Assets.Scripts.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Path
{
    public class PathFinder
    {
        private readonly Dictionary<Vector3Int, NavSurface> _surfaceGrid = new();
        private readonly List<PathNode> _candidates = new();
        private readonly List<PathNode> _processed = new();
        private readonly List<Vector3> _tempNeigbours = new();

        private readonly NavObstacleMap _obstacleMap;

        private readonly float _cellSize = 1;

        public PathFinder(NavObstacleMap obstacleMap, params NavSurface[] surfaces)
        {
            _obstacleMap = obstacleMap;

            foreach (var surface in surfaces)
            {
                var position = surface.Position;
                _surfaceGrid[position.FloorToInt(_cellSize)] = surface;
            }
        }

        public bool CalculateNewPathNonAlloc(Vector3 start, Vector3 destination, List<Vector3> path)
        {
            ClearCollections(path);

            if (!TryGetSurface(start, out var surface))
                return false;

            var startNode = CreateNode(null, start, destination);
            _candidates.Add(startNode);

            if (!FindBestPath(surface, destination, out var goal))
                return false;

            BuildPath(destination, path, goal);

            return true;
        }

        private void ClearCollections(List<Vector3> path)
        {
            _candidates.Clear();
            _processed.Clear();
            path.Clear();
        }

        private bool TryGetSurface(Vector3 position, out NavSurface surface)
        {
            var cell = position.FloorToInt(_cellSize);

            if (!_surfaceGrid.TryGetValue(cell, out surface))
                return false;

            return true;
        }

        private bool FindBestPath(NavSurface surface, Vector3 destination, out PathNode goal)
        {
            goal = null;

            var neigbours = _tempNeigbours;
            var candidates = _candidates;
            var processed = _processed;

            while (_candidates.Count > 0)
            {
                PathNode bestCandidate = GetBestNodeFromCandidates(candidates);

                if (IsEquals(bestCandidate, destination))
                {
                    goal = bestCandidate;
                    return true;
                }

                candidates.Remove(bestCandidate);
                processed.Add(bestCandidate);

                neigbours.Clear();

                if (!surface.TryGetNeigbours(bestCandidate.Position, neigbours))
                    return false;

                foreach (var neigbour in neigbours)
                {
                    if (processed.Exists(node => node.Position == neigbour))
                        continue;

                    var node = candidates.Find(node => node.Position == neigbour);

                    if (node == null)
                    {
                        node = CreateNode(bestCandidate, neigbour, destination);

                        if (IsNodeBlocked(neigbour))
                        {
                            processed.Add(node);
                            continue;
                        }

                        candidates.Add(node);
                    }
                    else
                        UpdateNodesData(destination, bestCandidate, neigbour, node);
                }
            }

            return false;
        }

        private PathNode GetBestNodeFromCandidates(List<PathNode> candidates)
        {
            PathNode bestCandidate = candidates[0];
            var bestCandidateValue = bestCandidate.TotalCost;

            foreach (var candidate in candidates)
            {
                if (candidate.TotalCost < bestCandidateValue)
                {
                    bestCandidate = candidate;
                    bestCandidateValue = candidate.TotalCost;
                }
            }

            return bestCandidate;
        }

        private bool IsEquals(PathNode node, Vector3 position)
        {
            return node.Position == position;
        }

        private PathNode CreateNode(PathNode parent, Vector3 position, Vector3 destination)
        {
            PathNode node;

            if (parent == null)
            {
                var totalDistance = Vector3.SqrMagnitude(destination - position);
                node = new(position, 0f, totalDistance, totalDistance, null);
            }
            else if (position == destination)
            {
                var lastDistance = Vector3.SqrMagnitude(position - parent.Position);
                var totalCost = parent.CostFromStart + lastDistance;
                node = new(position, totalCost, 0f, totalCost, parent);
            }
            else
            {
                var distanceToTarget = Vector3.SqrMagnitude(destination - position);
                var lastDistance = Vector3.SqrMagnitude(position - parent.Position);
                var costFromStart = parent.CostFromStart + lastDistance;
                node = new(position, costFromStart, distanceToTarget, costFromStart + distanceToTarget, parent);
            }

            return node;
        }

        private bool IsNodeBlocked(Vector3 neigbour)
        {
            if (!_obstacleMap.TryGetNavOstacles(neigbour, out var obstacles))
                return false;

            foreach (var obstacle in obstacles)
            {
                if (CollisionCalculator.Intersect())
                    return true;
            }

            return false;
        }

        private void UpdateNodesData(Vector3 destination, PathNode bestCandidate, Vector3 neigbour, PathNode node)
        {
            var distanseToParent = Vector3.SqrMagnitude(bestCandidate.Position - neigbour);
            var distanceFromStart = bestCandidate.CostFromStart + distanseToParent;

            if (node.CostFromStart > distanceFromStart)
            {
                node.CostFromStart = distanceFromStart;

                node.Parent = bestCandidate;

                var distanceToTarget = Vector3.SqrMagnitude(neigbour - destination);
                node.DistanceToTargetSq = distanceToTarget;

                node.TotalCost = distanceFromStart + distanceToTarget;
            }
        }

        private void BuildPath(Vector3 destination, List<Vector3> path, PathNode goal)
        {
            var node = goal;
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Parent;
            }

            path.Reverse();

            path.Add(destination);
        }
    }
}
