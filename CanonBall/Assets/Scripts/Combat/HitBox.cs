using Assets.Scripts.Creations;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HitBox : IComponent
    {

        private readonly Collider[] _colliders;
        private readonly List<EdgeChain> _edgeChains = new();
        private readonly List<Vector3> _cornerOffsetVectors = new();
        private readonly IHasPosition _center;

        public HitBox(IHasPosition center, Transform[] attackCorners, params Collider[] colliders)
        {
            _center = center;

            _edgeChains.Add(new EdgeChain(attackCorners));

            _colliders = new Collider[colliders.Length];

            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                _colliders[i] = collider;
            }

            var centerPosition = _center.Position;
            foreach (var corner in attackCorners)
            {
                var projection = Vector3.ProjectOnPlane(corner.position - centerPosition, Vector3.up);
                _cornerOffsetVectors.Add(projection);
            }
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            Vector3 closestPoint = Vector3.zero;
            float closestDistance = float.MaxValue;
            foreach (var collider in _colliders)
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

        public bool TryGetFreeAttackPoisitionAround(float radius, Vector3 source, out Vector3 position, out HitBoxAttackPlaceReservation reservation)
        {
            var length = _edgeChains.Count;
            for (int i = 0; i < length; i++)
            {
                var edgeChain = _edgeChains[i];

                if (edgeChain.TryGetFreeAttackPoisitionAround(radius, source, out position, out var edgeReservation))
                {
                    reservation = new(edgeReservation, i);
                    return true;
                }
            }

            ExpandEdgeChains();
            return TryGetFreeAttackPoisitionAround(radius, source, out position, out reservation);
        }

        public void ReleaseReservation(HitBoxAttackPlaceReservation reservation)
        {
            var edgeChain = _edgeChains[reservation.RingIndex];
            edgeChain.ReleaseReservation(reservation.Reservation);
        }

        private void ExpandEdgeChains()
        {
            var defaultLenthBetweenRings = 0.5f;

            var center = _center.Position;

            var length = _cornerOffsetVectors.Count;

            var corners = new Transform[length];

            for (int i = 0; i < length; i++)
            {
                var cornerOffsetVector = _cornerOffsetVectors[i];
                var cornerDirection = Vector3.Normalize(cornerOffsetVector);

                var corner = new GameObject();

                var cornerTransform = corner.transform;
                cornerTransform.position = (center + cornerOffsetVector) + _edgeChains.Count * defaultLenthBetweenRings * cornerDirection;

                corners[i] = cornerTransform;
            }

            _edgeChains.Add(new EdgeChain(corners));
        }
    }

    public readonly struct HitBoxAttackPlaceReservation
    {
        public EndgeChainPlaceReservation Reservation { get; }
        public int RingIndex { get; }

        public HitBoxAttackPlaceReservation(EndgeChainPlaceReservation reservation, int ringIndex)
        {
            Reservation = reservation;
            RingIndex = ringIndex;
        }
    }
}
