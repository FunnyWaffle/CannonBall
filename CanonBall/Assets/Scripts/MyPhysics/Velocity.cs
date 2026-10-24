using System;
using UnityEngine;

namespace Assets.Scripts.MyPhysics
{
    [Serializable]
    public class Velocity
    {
        [SerializeField] private Vector3 _max;
        [SerializeField] private float _maxHorizontalSpeed;
        [SerializeField] private Vector3 _min;
        [SerializeField] private float _maxVerticalSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;

        private Vector3 _current;

        public Vector3 Current => _current;
        public float CurrentSpeed => _current.magnitude;

        public Vector3 Increase(Vector3 direction, float maxSpeed = -1f)
        {
            if (maxSpeed < 0)
                maxSpeed = _max.magnitude;

            var speed = Change(maxSpeed, _acceleration);

            _current = direction * speed;

            return _current;
        }

        public Vector3 Decrease()
        {
            var minSpeed = _min.magnitude;

            var speed = Change(minSpeed, _deceleration);

            _current = _current.normalized * speed;

            return _current;
        }

        public void LimitSpeedRelativeTo(Quaternion rotation)
        {
            var local = Quaternion.Inverse(rotation) * _current;

            var limitX = GetLimit(local.x, _max.x, _min.x);
            var limitY = GetLimit(local.y, _max.y, _min.y);
            var limitZ = GetLimit(local.z, _max.z, _min.z);

            var scale = 1f;

            if (limitX > 0f)
                scale = Mathf.Max(scale, Mathf.Abs(local.x / limitX));

            if (limitY > 0f)
                scale = Mathf.Max(scale, Mathf.Abs(local.y / limitY));

            if (limitZ > 0f)
                scale = Mathf.Max(scale, Mathf.Abs(local.z / limitZ));

            local /= scale;

            _current = rotation * local;

            _current = Vector3.ClampMagnitude(_current, _maxHorizontalSpeed);
        }

        public void ResetCurrent()
        {
            _current = Vector3.zero;
        }

        public float CalculateTargetSpeedBy(float distance)
        {
            var allowedSpeed = GetAllowedSpeed(0f, distance);

            var targetSpeed = Mathf.Min(_max.magnitude, allowedSpeed);

            return targetSpeed;
        }

        public float CalculateTargetSpeedBy(
            float distance,
            float turnSpeed)
        {
            var allowedSpeed = GetAllowedSpeed(turnSpeed, distance);

            var targetSpeed = Mathf.Min(_max.magnitude, allowedSpeed);

            return targetSpeed;
        }

        private float Change(float target, float delta)
        {
            return Mathf.MoveTowards(
                       CurrentSpeed,
                       target,
                       delta * Time.deltaTime);
        }

        private float GetAllowedSpeed(float turnSpeed, float distance)
        {
            return Mathf.Sqrt(turnSpeed * turnSpeed
                + 2f * _deceleration * distance);
        }

        public float GetLimit(float value, float max, float min)
        {
            return value >= 0 ? max : min;
        }
    }
}
