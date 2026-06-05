using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HitBox
    {

        private readonly Collider[] _collider;
        private readonly EdgeChain _edgeChain;

        public HitBox(Collider[] colliders, Transform[] attackCorners)
        {
            _collider = new Collider[colliders.Length];

            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                _collider[i] = collider;
            }

            _edgeChain = new EdgeChain(attackCorners);
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            Vector3 closestPoint = Vector3.zero;
            float closestDistance = float.MaxValue;
            foreach (var collider in _collider)
            {
                var point = collider.ClosestPoint(position);
                var distance = Vector3.SqrMagnitude(point - position);
                if (distance < closestDistance)
                {
                    closestPoint = point;
                    closestDistance = distance;
                }
            }

            return closestPoint;
        }

        public bool TryGetFreePoisitionAround(float radius, Vector3 source, out Vector3 position)
        {
            AttackZoneEdge closestAttackZoneEdge = null;
            var distance = float.MaxValue;

            var diameter = radius * 2;

            var _attackEdges = _edgeChain.Edges;

            foreach (var edge in _attackEdges)
            {
                var currentDistance = Vector3.SqrMagnitude(edge.Center - source);
                if (currentDistance < distance
                    && edge.HasFreeSpace(diameter))
                {
                    closestAttackZoneEdge = edge;
                    distance = currentDistance;
                }
            }

            if (closestAttackZoneEdge == null)
            {
                position = default;
                return false;
            }

            position = closestAttackZoneEdge.GetFreePosition(radius, source, out var reservation);
            return true;
        }
    }
}
