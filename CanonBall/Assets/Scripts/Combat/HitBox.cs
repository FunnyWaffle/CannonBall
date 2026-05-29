using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HitBox
    {

        private readonly Collider[] _collider;
        private readonly AttackZoneEdge[] _attackEdges;

        public HitBox(Collider[] colliders, AttackZoneEdge[] attackEdges)
        {
            _collider = new Collider[colliders.Length];

            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                _collider[i] = collider;
            }

            _attackEdges = new AttackZoneEdge[attackEdges.Length];

            for (int i = 0; i < attackEdges.Length; i++)
            {
                var edge = attackEdges[i];
                edge.Initialize();
                _attackEdges[i] = edge;
            }
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
            AttackZoneEdge closestAttackZoneEdge = _attackEdges[0];
            var distance = float.MaxValue;

            var diameter = radius * 2;

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

            position = closestAttackZoneEdge.GetFreePosition(radius, source);
            return false;
        }

        //public bool TryGetFreePoisitionAround(float radius, Vector3 source, out Vector3 position)
        //{
        //    AttackZoneEdge closestAttackZoneEdge = _attackEdges[0];
        //    var distance = float.MaxValue;

        //    var diameter = radius * 2;

        //    foreach (var edge in _attackEdges)
        //    {
        //        if (closestAttackZoneEdge.TryGetFreePosition(radius, source, out position))
        //            return true;
        //    }

        //    position = default;
        //    return false;
        //}
    }
}
