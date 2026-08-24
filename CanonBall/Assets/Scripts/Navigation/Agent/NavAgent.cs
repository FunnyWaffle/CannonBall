using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Navigation.Agent
{
    public class NavAgent : MonoBehaviour
    {
        [SerializeField] private float _acceleration;
        [SerializeField] private float _decceleration;
        [SerializeField] private Vector3 _maxVelocity;

        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _distanceToTurn = .1f;

        [SerializeField] private Collider[] _movementCollision;

        private Vector3 _currentVelocity;

        public Vector3 Position => transform.position;
        public Collider[] MovementCollision => _movementCollision;

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

                var turnSpeedMultiplyer = 1f;
                if (i < count - 1)
                {
                    var nextCorner = path[i + 1];
                    var nextCornerDirection = Vector3.Normalize(nextCorner - corner);
                    var angle = Vector3.Angle(direction, nextCornerDirection);
                    turnSpeedMultiplyer = Mathf.Clamp01(1 - angle / 90f);
                }

                while (distanceToCorner > _distanceToTurn)
                {
                    distanceToCorner = Vector3.Distance(transform.position, corner);
                    RotateTo(direction);
                    MoveTo(direction, turnSpeedMultiplyer, distanceToCorner);

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

        private void MoveTo(Vector3 direction, float turnSpeedMultiplyer, float distanceToCorner)
        {
            var currentVelocity = _currentVelocity;

            var currentSpeed = currentVelocity.magnitude;
            var maxSpeed = _maxVelocity.magnitude;

            var acceleration = _acceleration;
            var deceleration = _decceleration;

            var turnSpeed = maxSpeed * turnSpeedMultiplyer;

            var targetSpeed = CalculateTargetSpeed(turnSpeed, deceleration, distanceToCorner, maxSpeed);

            var velocityDelta = targetSpeed < currentSpeed
                ? deceleration
                : acceleration;

            var targetVelocity = direction * targetSpeed;

            var deltaTime = Time.deltaTime;

            _currentVelocity = Vector3.MoveTowards(
                        currentVelocity,
                        targetVelocity,
                        velocityDelta * deltaTime);

            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, maxSpeed);

            transform.position += _currentVelocity * deltaTime;
        }

        private float CalculateTargetSpeed(
            float turnSpeed,
            float deceleration,
            float distanceToCorner,
            float maxSpeed)
        {
            var allowedSpeed = Mathf.Sqrt(
                turnSpeed * turnSpeed
                + 2f * deceleration * distanceToCorner);

            var targetSpeed = Mathf.Min(maxSpeed, allowedSpeed);

            return targetSpeed;
        }
    }
}
