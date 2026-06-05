using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class AttackZoneEdge
    {
        private readonly Transform _startCorner;
        private readonly Transform _endCorner;

        private readonly List<FreeSegment> _freeSegments = new();

        private static readonly float _mergeEpsilon = 0.001f;

        private float _largestSegmentLength;

        public AttackZoneEdge(Transform startCorner, Transform endCorner)
        {
            _startCorner = startCorner;
            _endCorner = endCorner;

            var segment = new FreeSegment(0, Vector3.Distance(_endCorner.position, _startCorner.position));
            _freeSegments.Add(segment);
            _largestSegmentLength = segment.Length;
        }

        public Vector3 Center => (_startCorner.position + _endCorner.position) / 2;

        public bool HasFreeSpace(float diameter)
        {
            return _largestSegmentLength >= diameter;
        }

        public bool TryGetFreePosition(
            float radius,
            Vector3 source,
            out Vector3 position,
            out Reservation reservation)
        {
            if (!HasFreeSpace(radius * 2))
            {
                position = Vector3.zero;
                reservation = default;
                return false;
            }

            position = GetFreePosition(radius, source, out reservation);
            return true;
        }

        public Vector3 GetFreePosition(
            float radius,
            Vector3 source,
            out Reservation reservation)
        {
            var start = _startCorner.position;
            var axis = _endCorner.position - start;
            var length = Vector3.Magnitude(axis);
            var direction = axis / length;

            var closestPoint = ProjectDistanceOnEdge(source, start, axis, length);
            closestPoint = ClampPoint(radius, closestPoint, length);

            var segment = FindNearestFreeSegment(closestPoint, radius, out var index);
            var position = StickPositionInSegment(radius, start, direction, closestPoint, segment, out var segmentSeparationCenter);

            reservation = RecalculateFreeSpace(segment, index, segmentSeparationCenter, radius);
            return position;
        }

        public void Release(Reservation reservation)
        {
            var pointStart = reservation.Start;
            var pointEnd = reservation.End;

            var endIndex = FindInsertionIndex(pointStart);
            var startIndex = endIndex - 1;

            bool hasLeft = startIndex >= 0;
            bool hasRight = endIndex < _freeSegments.Count;

            var touchLeft = false;
            var touchRight = false;

            FreeSegment leftSegment = default;
            FreeSegment rightSegment = default;
            FreeSegment newSegment = default;

            if (hasLeft)
            {
                leftSegment = _freeSegments[startIndex];
                touchLeft = Mathf.Abs(leftSegment.End - pointStart) <= _mergeEpsilon;
            }

            if (hasRight)
            {
                rightSegment = _freeSegments[endIndex];
                touchRight = Mathf.Abs(rightSegment.Start - pointEnd) <= _mergeEpsilon;
            }

            if (touchLeft && touchRight)
            {
                _freeSegments.RemoveAt(endIndex);
                _freeSegments.RemoveAt(startIndex);

                newSegment = new FreeSegment(leftSegment.Start, rightSegment.End);
                _freeSegments.Insert(startIndex, newSegment);
            }
            else if (touchLeft)
            {
                _freeSegments.RemoveAt(startIndex);

                newSegment = new FreeSegment(leftSegment.Start, pointEnd);
                _freeSegments.Insert(startIndex, newSegment);
            }
            else if (touchRight)
            {
                _freeSegments.RemoveAt(endIndex);

                newSegment = new FreeSegment(pointStart, rightSegment.End);
                _freeSegments.Insert(endIndex, newSegment);
            }
            else
            {
                newSegment = new FreeSegment(pointStart, pointEnd);
                _freeSegments.Insert(endIndex, newSegment);
            }

            if (newSegment.Length >= _largestSegmentLength)
            {
                _largestSegmentLength = newSegment.Length;
            }
        }

        private float ProjectDistanceOnEdge(
            Vector3 point,
            Vector3 start,
            Vector3 edge,
            float length)
        {
            var sqrLength = length * length;

            float t =
                Vector3.Dot(point - start, edge)
                / sqrLength;

            t = Mathf.Clamp01(t);

            return length * t;
        }

        private float ClampPoint(float radius, float closestPoint, float lenght)
        {
            return Mathf.Clamp(closestPoint, radius, lenght - radius);
        }

        private FreeSegment FindNearestFreeSegment(float point, float radius, out int index)
        {
            var pointStart = point - radius;
            var diameter = radius * 2;

            var insertIndex = FindInsertionIndex(pointStart);

            var left = insertIndex - 1;
            var right = insertIndex;

            var segmentCount = _freeSegments.Count;

            while (left >= 0 || right < segmentCount)
            {
                if (left >= 0)
                {
                    var tempSegment = _freeSegments[left];

                    if (diameter <= tempSegment.Length)
                    {
                        index = left;
                        return tempSegment;
                    }

                    left--;
                }

                if (right < segmentCount)
                {
                    var tempSegment = _freeSegments[right];

                    if (diameter <= tempSegment.Length)
                    {
                        index = right;
                        return tempSegment;
                    }

                    right++;
                }
            }

            throw new Exception("Segment that can contains an object not found! Must be called HasFreeSpace method before.");
        }

        private Vector3 StickPositionInSegment
            (float radius,
            Vector3 start,
            Vector3 direction,
            float closestPoint,
            FreeSegment segment,
            out float center)
        {
            var validMin = segment.Start + radius;
            var validMax = segment.End - radius;

            center = Mathf.Clamp(
               closestPoint,
               validMin,
               validMax);

            return start + direction * center;
        }

        private Reservation RecalculateFreeSpace(FreeSegment segment, int index, float point, float radius)
        {
            _freeSegments.RemoveAt(index);

            var start = segment.Start;
            var pointEnd = point + radius;
            float pointStart = point - radius;

            var leftSegment = new FreeSegment(start, pointStart);
            var rightSegment = new FreeSegment(pointEnd, segment.End);


            if (leftSegment.Length > 0)
            {
                index = FindInsertionIndex(start);
                _freeSegments.Insert(index, leftSegment);
            }

            if (rightSegment.Length > 0)
            {
                index = FindInsertionIndex(pointEnd);
                _freeSegments.Insert(index, rightSegment);
            }

            if (segment.Length >= _largestSegmentLength)
            {
                RecalculateLargestSegment();
            }

            return new Reservation(pointStart, pointEnd);
        }

        private void RecalculateLargestSegment()
        {
            _largestSegmentLength = 0;
            foreach (var segment in _freeSegments)
            {
                var length = segment.Length;
                if (length > _largestSegmentLength)
                {
                    _largestSegmentLength = length;
                }
            }
        }

        private int FindInsertionIndex(float start)
        {
            int low = 0;
            int high = _freeSegments.Count;

            while (low < high)
            {
                int mid = (low + high) / 2;

                if (_freeSegments[mid].Start < start)
                    low = mid + 1;
                else
                    high = mid;
            }

            return low;
        }
    }

    public readonly struct FreeSegment
    {
        public float Start { get; }
        public float End { get; }
        public float Length { get; }


        public FreeSegment(float start, float end)
        {
            Start = start;
            End = end;

            Length = end - start;
        }
    }

    public readonly struct Reservation
    {
        public float Start { get; }
        public float End { get; }

        public Reservation(float start, float end)
        {
            Start = start;
            End = end;
        }
    }
}
