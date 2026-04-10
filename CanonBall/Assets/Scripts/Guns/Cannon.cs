using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class Cannon
    {
        private readonly CannonRotator _rotator;
        private readonly CannonShooter _shooter;
        private readonly FirstPersonCannonCrosshairPreview _firstPersonCrosshairPreview;
        private readonly ThirdPersonCannonCrosshairPreview _thirdPersonCrosshairPreview;

        private CrosshairMode _crosshairMode = CrosshairMode.FirstPerson;

        public Cannon(CannonRotator rotator,
            CannonShooter shooter,
            FirstPersonCannonCrosshairPreview firstPersonCannonCrosshairPreview,
            ThirdPersonCannonCrosshairPreview thirdPersonCannonCrosshairPreview)
        {
            _rotator = rotator;
            _shooter = shooter;
            _firstPersonCrosshairPreview = firstPersonCannonCrosshairPreview;
            _thirdPersonCrosshairPreview = thirdPersonCannonCrosshairPreview;
        }

        public void Shoot()
        {
            _shooter.Shoot();
        }

        public void RotateToPosition(Vector3 position)
        {
            var barrelExitPosition = _shooter.BarrelExitPosition;
            float shootPower = _shooter.ShootPower;

            var velocityToTarget = CannonShootTrajectoryCalculator
                .GetVelocity(position, barrelExitPosition, shootPower);
            _rotator.RotateInDirection(velocityToTarget);

            var trajectory = GetCurrentTrajectoryPrediction(barrelExitPosition, shootPower);
            var hitPoint = trajectory[^1];
            MoveCrosshair(hitPoint);

            VisualizeTrajectory(trajectory);
        }

        private List<Vector3> GetCurrentTrajectoryPrediction(Vector3 barrelExitPosition, float shootPower)
        {
            var realVelocity = _shooter.BarrelForward * shootPower;

            var currentTrajectory = CannonShootTrajectoryCalculator
                 .GetTrajectory(barrelExitPosition, realVelocity);
            return currentTrajectory;
        }

        public void SetCrosshairType(int crosshairMode)
        {
            switch (crosshairMode)
            {
                case 0:
                    _firstPersonCrosshairPreview.SetActive(true);
                    _thirdPersonCrosshairPreview.SetActive(false);
                    _crosshairMode = CrosshairMode.FirstPerson;
                    break;
                case 1:
                    _crosshairMode = CrosshairMode.ThirdPerson;
                    _firstPersonCrosshairPreview.SetActive(false);
                    _thirdPersonCrosshairPreview.SetActive(true);
                    break;
            }
        }

        private void MoveCrosshair(Vector3 hitPoint)
        {
            if (_crosshairMode == CrosshairMode.FirstPerson)
                _firstPersonCrosshairPreview.SetPosition(hitPoint);
            else
                _thirdPersonCrosshairPreview.SetPosition(hitPoint);
        }

        private void VisualizeTrajectory(List<Vector3> trajectory)
        {
            for (int i = 0; i < trajectory.Count - 1; i++)
            {
                Debug.DrawLine(trajectory[i], trajectory[i + 1]);
            }

        }
    }

    public enum CrosshairMode
    {
        FirstPerson,
        ThirdPerson,
    }
}
