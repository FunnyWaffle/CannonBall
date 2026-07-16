using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class EdgeChain
    {
        private readonly List<AttackZoneEdge> _edges = new();

        public EdgeChain(Transform[] attackCorners)
        {
            _edges = new List<AttackZoneEdge>();
            InitializeEdges(attackCorners);
        }

        public bool TryGetFreeAttackPoisitionAround(float radius, Vector3 source, out Vector3 position, out EndgeChainPlaceReservation reservation)
        {
            AttackZoneEdge closestAttackZoneEdge = null;
            var distance = float.MaxValue;
            var id = 0;

            var diameter = radius * 2;

            var attackEdges = _edges;

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

        public void ReleaseReservation(EndgeChainPlaceReservation reservation)
        {
            var edge = _edges[reservation.EdgeIndex];
            edge.Release(reservation.Reservation);
        }

        private void InitializeEdges(Transform[] attackCorners)
        {
            int length = attackCorners.Length;

            var firstCorner = attackCorners[0];

            for (int i = 0, lastIndex = length - 1; i < length; i++)
            {
                var currentCorner = attackCorners[i];
                Transform endCorner;

                if (i == lastIndex)
                {
                    var lastToFirstCornerAxis = Vector3.Normalize(currentCorner.position - firstCorner.position);
                    var secondToFirstCornerAxis = Vector3.Normalize(attackCorners[1].position - firstCorner.position);
                    if (lastToFirstCornerAxis == secondToFirstCornerAxis)
                        return;

                    endCorner = firstCorner;
                }
                else
                    endCorner = attackCorners[i + 1];

                if (Physics.Linecast(currentCorner.position, endCorner.position, out var hit))
                    if (!hit.collider.isTrigger)
                        continue;

                _edges.Add(new AttackZoneEdge(currentCorner, endCorner));
            }
        }
    }

    public readonly struct EndgeChainPlaceReservation
    {
        public Reservation Reservation { get; }
        public int EdgeIndex { get; }

        public EndgeChainPlaceReservation(Reservation reservation, int edgeIndex)
        {
            Reservation = reservation;
            EdgeIndex = edgeIndex;
        }
    }
}
