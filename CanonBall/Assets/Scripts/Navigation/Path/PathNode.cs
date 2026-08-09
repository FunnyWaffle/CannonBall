using UnityEngine;

namespace Assets.Scripts.Navigation.Path
{
    public class PathNode
    {
        public PathNode(
            Vector3 position,
            float costFromStart,
            float distanceToTargetSq,
            float totalCost,
            PathNode previousPosition)
        {
            Position = position;
            CostFromStart = costFromStart;
            DistanceToTargetSq = distanceToTargetSq;
            TotalCost = totalCost;
            Parent = previousPosition;
        }

        public Vector3 Position { get; }

        public float CostFromStart;
        public float DistanceToTargetSq;
        public float TotalCost;

        public PathNode Parent;
    }
}
