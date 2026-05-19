using Assets.Scripts.Systems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns.Components
{
    public class CannonRotator
    {
        private readonly CrosshairSystem _crosshairSystem;

        private float _rotationSpeed = 1f;
        private float _pitchAngleLimit = 15;

        private Quaternion _rotation;

        public CannonRotator(
            CrosshairSystem crosshairSystem,
            float rotationSpeed,
            float pitchAngleLimit,
            Quaternion startRotation)
        {
            _crosshairSystem = crosshairSystem;
            _rotationSpeed = rotationSpeed;
            _pitchAngleLimit = pitchAngleLimit;
            _rotation = startRotation;
        }

        public void SetRotationSpeed(float value)
        {
            _rotationSpeed = value;
        }

        public void SetPitchLimit(float value)
        {
            _pitchAngleLimit = value;
        }

        public Quaternion Rotate(Vector3 positionToRotate, Vector3 barrelExitPosition, Vector3 barrelForward, float shootPower)
        {
            var velocityToTarget = CannonShootTrajectoryCalculator
                .GetVelocity(positionToRotate, barrelExitPosition, shootPower);

            Quaternion targetRotationQuaternion = ClampDirection(velocityToTarget);

            _rotation = Quaternion.RotateTowards(
                _rotation,
                targetRotationQuaternion,
                _rotationSpeed * Time.deltaTime);


            var trajectory = GetCurrentTrajectoryPrediction(barrelExitPosition, barrelForward, shootPower);
            var hitPoint = trajectory[^1];
            _crosshairSystem.SetCrosshairPosition(hitPoint);

            VisualizeTrajectory(trajectory);

            return _rotation;
        }

        private List<Vector3> GetCurrentTrajectoryPrediction(Vector3 barrelExitPosition, Vector3 barrelForward, float shootPower)
        {
            var realVelocity = barrelForward * shootPower;

            var currentTrajectory = CannonShootTrajectoryCalculator
                 .GetTrajectory(barrelExitPosition, realVelocity);
            return currentTrajectory;
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

        private void VisualizeTrajectory(List<Vector3> trajectory)
        {
            for (int i = 0; i < trajectory.Count - 1; i++)
            {
                Debug.DrawLine(trajectory[i], trajectory[i + 1]);
            }
        }
    }
}
