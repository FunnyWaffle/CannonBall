using Assets.Scripts.Creations;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HitBox : IComponent
    {

        private readonly Collider[] _collider;
        private readonly EdgeChain _edgeChain;

        public HitBox(Transform[] attackCorners, params Collider[] colliders)
        {
            _edgeChain = new EdgeChain(attackCorners);

            _collider = new Collider[colliders.Length];

            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                _collider[i] = collider;
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

        public bool TryGetFreePoisitionAround(float radius, Vector3 source, out Vector3 position, out HitBoxAttackPlaceReservation reservation)
        {
            AttackZoneEdge closestAttackZoneEdge = null;
            var distance = float.MaxValue;
            var id = 0;

            var diameter = radius * 2;

            var attackEdges = _edgeChain.Edges;

            for (int i = 0; i < attackEdges.Count; i++)
            {
                var edge = attackEdges[i];

                var currentDistance = Vector3.SqrMagnitude(edge.Center - source);
                if (currentDistance < distance
                    && edge.HasFreeSpace(diameter))
                {
                    closestAttackZoneEdge = edge;
                    distance = currentDistance;
                    id = i;
                }
            }

            if (closestAttackZoneEdge == null)
            {
                position = default;
                reservation = default;
                return false;
            }

            position = closestAttackZoneEdge.GetFreePosition(radius, source, out var endgeReservation);
            reservation = new(endgeReservation, id);
            return true;
        }

        public void ReleaseReservation(HitBoxAttackPlaceReservation reservation)
        {
            var edges = _edgeChain.Edges;
            var edge = edges[reservation.EdgeIndex];
            edge.Release(reservation.Reservation);
        }
    }

    public readonly struct HitBoxAttackPlaceReservation
    {
        public Reservation Reservation { get; }
        public int EdgeIndex { get; }

        public HitBoxAttackPlaceReservation(Reservation reservation, int edgeIndex)
        {
            Reservation = reservation;
            EdgeIndex = edgeIndex;
        }
    }
}
