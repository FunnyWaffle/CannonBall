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

        public CannonRotator Rotator => _rotator;

        public void Shoot()
        {
            _shooter.Shoot();
        }

        public void RotateToPosition(Vector3 position)
        {
            var barrelExitPosition = _shooter.BarrelExitPosition;

            var trajectory = CannonShootTrajectoryCalculator
                .GetTrajectory(position, barrelExitPosition, _shooter.ShootPower, out var velocity);
            _rotator.RotateToPosition(barrelExitPosition + velocity);

            if (_crosshairMode == CrosshairMode.FirstPerson)
                _firstPersonCrosshairPreview.SetPosition(_shooter.GetFacedPosition());
            else
                _thirdPersonCrosshairPreview.SetPosition(trajectory[^1]);
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

        public enum CrosshairMode
        {
            FirstPerson,
            ThirdPerson,
        }
    }
}
