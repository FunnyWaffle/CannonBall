using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    public static class ListExtensions
    {
        public static Vector3 GetClosest(this IEnumerable<Vector3> points, Vector3 position, out float distanceSqr)
        {
            var closest = Vector3.zero;
            distanceSqr = float.MaxValue;

            foreach (var point in points)
            {
                var currentDistance = Vector3.SqrMagnitude(point - position);

                if (currentDistance < distanceSqr)
                {
                    currentDistance = distanceSqr;
                    closest = point;
                }
            }

            return closest;
        }
    }
}
