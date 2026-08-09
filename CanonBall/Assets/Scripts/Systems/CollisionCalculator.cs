using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public static class CollisionCalculator
    {
        private static readonly RaycastHit[] _hitedColliders = new RaycastHit[10];
        private static readonly HashSet<Collider> _collidedColliders = new();

        public static bool Intersect(
            Collider[] sourceColliders,
            Collider[] obstacleColliders,
            Vector3 from, Vector3 to)
        {
            _collidedColliders.Clear();

            var axis = to - from;
            var length = Vector3.Magnitude(axis);

            if (length <= Mathf.Epsilon)
                return false;

            var direction = axis / length;
            var orientation = Quaternion.LookRotation(direction, Vector3.up);

            foreach (var sourceCollider in sourceColliders)
            {
                var transform = sourceCollider.transform;
                var localRotation = transform.localRotation;
                var rotation = orientation * localRotation;

                var hitColliderCount = 0;

                if (TryCast(sourceCollider, out BoxCollider boxCollider))
                {
                    var center = transform.localPosition + localRotation * boxCollider.center;
                    var position = from + orientation * center;

                    hitColliderCount = Physics.BoxCastNonAlloc(position,
                        boxCollider.size * 0.5f,
                        direction,
                        _hitedColliders,
                        rotation,
                        length);
                }
                else if (TryCast(sourceCollider, out SphereCollider sphereCollider))
                {
                    var center = transform.localPosition + localRotation * sphereCollider.center;
                    var position = from + orientation * center;

                    hitColliderCount = Physics.SphereCastNonAlloc(position, sphereCollider.radius, direction, _hitedColliders, length);
                }
                else if (TryCast(sourceCollider, out CapsuleCollider capsuleCollider))
                {
                    var center = transform.localPosition + localRotation * capsuleCollider.center;
                    var radius = capsuleCollider.radius;

                    var position = from + orientation * center;

                    var capsuleDirection = capsuleCollider.direction switch
                    {
                        0 => Vector3.right,
                        1 => Vector3.up,
                        2 => Vector3.forward,
                    };

                    var capsuleAxis = rotation * capsuleDirection;

                    var cylinderHalfHeight = capsuleCollider.height * 0.5f - radius;

                    var point1 = position + capsuleAxis * cylinderHalfHeight;
                    var point2 = position - capsuleAxis * cylinderHalfHeight;

                    hitColliderCount = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, _hitedColliders, length);
                }

                if (hitColliderCount == 0)
                    continue;

                for (int i = 0; i < hitColliderCount; i++)
                {
                    _collidedColliders.Add(_hitedColliders[i].collider);
                }
            }

            foreach (var collidedCollider in _collidedColliders)
            {
                if (obstacleColliders.Contains(collidedCollider))
                    return true;
            }

            return false;
        }

        private static bool TryCast<T>(Collider collider, out T сollider)
            where T : Collider
        {
            if (collider is T typedCollider)
            {
                сollider = typedCollider;
                return true;
            }

            сollider = default;
            return false;
        }
    }
}
