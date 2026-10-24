using Assets.Scripts.MyPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Agent
{
    public class NavAgent : MonoBehaviour
    {
        [SerializeField] private Velocity _velocity;

        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _distanceToTurn = .1f;
        [SerializeField] private float _stepHeight = .1f;

        [SerializeField] private Collider[] _movementCollision;

        public Vector3 Position => transform.position;
        public Collider[] MovementCollision => _movementCollision;

        public float StepHeight => _stepHeight;

        public event Action PathCompleted;

        public IEnumerator Set(IReadOnlyList<Vector3> path)
        {
            var enumerator = FollowPath(path);

            return enumerator;
        }

        private IEnumerator FollowPath(IReadOnlyList<Vector3> path)
        {
            var count = path.Count;

            for (int i = 0; i < count; i++)
            {
                var corner = path[i];
                var distanceToCorner = Vector3.Distance(transform.position, corner);
                var direction = Vector3.Normalize(corner - transform.position);

                while (distanceToCorner > _distanceToTurn)
                {
                    distanceToCorner = Vector3.Distance(transform.position, corner);
                    RotateTo(direction);
                    MoveTo(corner, direction, distanceToCorner);

                    yield return null;
                }
            }

            PathCompleted?.Invoke();
        }

        private void RotateTo(Vector3 direction)
        {
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        rotation,
                        _angularSpeed * Time.deltaTime);
        }

        private void MoveTo(Vector3 position, Vector3 direction, float distanceToCorner)
        {
            var targetSpeed = _velocity.CalculateTargetSpeedBy(distanceToCorner);

            var deltaTime = Time.deltaTime;

            if (targetSpeed > _velocity.CurrentSpeed)
                _velocity.Increase(direction, targetSpeed);
            else
                _velocity.Decrease();

            _velocity.LimitSpeedRelativeTo(transform.rotation);

            var delta = _velocity.Current * deltaTime;

            if (WillCross(distanceToCorner, delta))
            {
                transform.position = position;
                _velocity.ResetCurrent();
                return;
            }

            transform.position += delta;
        }

        private bool WillCross(float distanceToCorner, Vector3 displacement)
        {
            return distanceToCorner * distanceToCorner <= displacement.sqrMagnitude;
        }
    }
}
