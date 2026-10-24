using Assets.Scripts.Navigation.Agent;
using Assets.Scripts.Navigation.Surface;
using Assets.Scripts.Systems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Path
{
    public class AStar
    {
        private readonly List<PathNode> _candidates = new();
        private readonly List<PathNode> _processed = new();

        public bool FindBestPath(SurfaceSequence surfaceSequence,
            Vector3 start, Vector3 destination,
            NavAgent navAgent, out PathNode goal)
        {
            var agentColliders = navAgent.MovementCollision;
            var stepHeight = navAgent.StepHeight;

            ClearCollections();

            var startNode = CreateNode(null, start, destination);
            _candidates.Add(startNode);

            goal = null;

            var candidates = _candidates;
            var processed = _processed;

            if (!surfaceSequence.GetNodesIn(destination, out var destinationNodes))
                return false;

            while (_candidates.Count > 0)
            {
                PathNode bestCandidate = GetBestNodeFromCandidates(candidates);

                foreach (var node in destinationNodes)
                {
                    if (IsEquals(bestCandidate, node))
                    {
                        if (IsNodeBlocked(bestCandidate.Position, node, agentColliders))
                            goal = GetBestNodeFromCandidates(processed);
                        else
                            goal = CreateNode(bestCandidate, node, destination);

                        return true;
                    }
                }

                candidates.Remove(bestCandidate);
                processed.Add(bestCandidate);

                if (!surfaceSequence.GetNeigbourNodes(bestCandidate.Position, out var neigbours))
                    return false;

                foreach (var neigbour in neigbours)
                {
                    if (processed.Exists(node => node.Position == neigbour))
                        continue;

                    var from = bestCandidate.Position;
                    var to = neigbour;

                    if (!CanClimb(from, to, stepHeight)
                        || IsNodeBlocked(from, to, agentColliders))
                    {
                        continue;
                    }

                    var node = candidates.Find(node => node.Position == neigbour);

                    if (node == null)
                    {
                        node = CreateNode(bestCandidate, neigbour, destination);
                        candidates.Add(node);
                    }
                    else
                        UpdateNodesData(destination, bestCandidate, neigbour, node);
                }

                surfaceSequence.Return(neigbours);
            }

            return false;
        }

        private void ClearCollections()
        {
            _candidates.Clear();
            _processed.Clear();
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
           => node.Position == position;

        private PathNode CreateNode(PathNode parent, Vector3 position, Vector3 destination)
        {
            PathNode node;

            if (parent == null)
            {
                var totalDistance = Vector3.Distance(destination, position);
                node = new(position, 0f, totalDistance, totalDistance, null);
            }
            else if (position == destination)
            {
                var lastDistance = Vector3.Distance(position, parent.Position);
                var totalCost = parent.CostFromStart + lastDistance;
                node = new(position, totalCost, 0f, totalCost, parent);
            }
            else
            {
                var distanceToTarget = Vector3.Distance(destination, position);
                var lastDistance = Vector3.Distance(position, parent.Position);
                var costFromStart = parent.CostFromStart + lastDistance;
                node = new(position, costFromStart, distanceToTarget, costFromStart + distanceToTarget, parent);
            }

            return node;
        }

        private bool CanClimb(Vector3 from, Vector3 to, float stepHeight)
        {
            var heightBetweenPoints = to.y - from.y;

            return heightBetweenPoints <= stepHeight;
        }

        private bool IsNodeBlocked(
            Vector3 from,
            Vector3 to,
            Collider[] agentColliders) => CollisionCalculator.Intersect(
                agentColliders,
                from,
                to);

        private void UpdateNodesData(Vector3 destination, PathNode bestCandidate, Vector3 neigbour, PathNode node)
        {
            var distanseToParent = Vector3.Distance(bestCandidate.Position, neigbour);
            var distanceFromStart = bestCandidate.CostFromStart + distanseToParent;

            if (node.CostFromStart > distanceFromStart)
            {
                node.CostFromStart = distanceFromStart;

                node.Parent = bestCandidate;

                var distanceToTarget = Vector3.Distance(neigbour, destination);
                node.DistanceToTargetSq = distanceToTarget;

                node.TotalCost = distanceFromStart + distanceToTarget;
            }
        }

    }
}
