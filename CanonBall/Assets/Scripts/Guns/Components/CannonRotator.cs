using System;
using UnityEngine;

namespace Assets.Scripts.Guns.Components
{
    public class CannonRotator
    {
        private float _rotationSpeed = 1f;
        private float _pitchAngleLimit = 15;

        private Quaternion _rotation;

        public CannonRotator(float rotationSpeed, float pitchAngleLimit, Quaternion startRotation)
        {
            _rotationSpeed = rotationSpeed;
            _pitchAngleLimit = pitchAngleLimit;
            Rotation = startRotation;
        }

        public Quaternion Rotation
        {
            get => _rotation; set
            {
                _rotation = value;
                Rotated?.Invoke(_rotation);
            }
        }

        public event Action<Quaternion> Rotated;

        public void SetRotationSpeed(float value)
        {
            _rotationSpeed = value;
        }

        public void SetPitchLimit(float value)
        {
            _pitchAngleLimit = value;
        }

        public void RotateInDirection(Vector3 direction)
        {
            Quaternion targetRotationQuaternion = ClampDirection(direction);

            Rotation = Quaternion.RotateTowards(
                Rotation,
                targetRotationQuaternion,
                _rotationSpeed * Time.deltaTime);
        }

        private Quaternion ClampDirection(Vector3 targetPosition)
        {
            var targetRotation = Quaternion.LookRotation(targetPosition).eulerAngles;

            var pitch = targetRotation.x;
            if (pitch > 180f)
                pitch -= 360;

            var clampedPitch = Mathf.Clamp(pitch, -_pitchAngleLimit, _pitchAngleLimit);

            return Quaternion.Euler(clampedPitch, targetRotation.y, 0f);
        }
    }
}
