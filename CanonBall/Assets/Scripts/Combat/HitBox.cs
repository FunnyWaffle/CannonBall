using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class HitBox
    {
        private readonly Collider[] _collider;

        public HitBox(Collider[] colliders)
        {
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
    }
}
