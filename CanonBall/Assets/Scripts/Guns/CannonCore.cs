using Assets.Scripts.Camera;
using Assets.Scripts.Creations.Player.Components;
using Assets.Scripts.Guns.Components;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class CannonCore
    {
        private readonly CannonRotator _rotator;
        private readonly CannonShooter _shooter;
        private readonly Aimer _aimer;

        private CrosshairMode _crosshairMode = CrosshairMode.FirstPerson;

        public CannonCore(CannonRotator rotator, CannonShooter shooter, Aimer aimer)
        {
            _rotator = rotator;
            _shooter = shooter;
            _aimer = aimer;
        }

        public CrosshairMode CurrentCrosshairMode
        {
            get => _crosshairMode;
            private set
            {
                _crosshairMode = value;
                CrosshairModeChanged.Invoke(value);
            }
        }
        public CannonShooter Shooter => _shooter;

        public ReactiveProperty<CameraViewType> CurrentViewType = new();
        public event Action<CrosshairMode> CrosshairModeChanged;
        public event Action<Vector3> CrosshairPositionChanged;

        public void Aim(Vector2 input)
        {
            _aimer.Aim(input);
        }

        public void RotateToPosition(Vector3 position)
        {
            var barrelExitPosition = Shooter.BarrelExitPosition;
            float shootPower = Shooter.ShootPower;

            var velocityToTarget = CannonShootTrajectoryCalculator
                .GetVelocity(position, barrelExitPosition, shootPower);
            _rotator.RotateInDirection(velocityToTarget);

            var trajectory = GetCurrentTrajectoryPrediction(barrelExitPosition, shootPower);
            var hitPoint = trajectory[^1];
            MoveCrosshair(hitPoint);

            VisualizeTrajectory(trajectory);
        }

        public void Shoot()
        {
            Shooter.Shoot();
        }

        public void SetCrosshairType(int crosshairMode)
        {
            CurrentCrosshairMode = crosshairMode switch
            {
                0 => CrosshairMode.FirstPerson,
                1 => CrosshairMode.ThirdPerson,
                _ => throw new NotImplementedException()
            };
        }

        public void SetViewMode(int viewModeIndex)
        {
            CurrentViewType.Value = viewModeIndex switch
            {
                0 => CameraViewType.FirstPerson,
                1 => CameraViewType.ThirdPerson,
                _ => throw new NotImplementedException(),
            };
        }

        private List<Vector3> GetCurrentTrajectoryPrediction(Vector3 barrelExitPosition, float shootPower)
        {
            var realVelocity = Shooter.BarrelForward * shootPower;

            var currentTrajectory = CannonShootTrajectoryCalculator
                 .GetTrajectory(barrelExitPosition, realVelocity);
            return currentTrajectory;
        }

        private void MoveCrosshair(Vector3 hitPoint)
        {
            CrosshairPositionChanged?.Invoke(hitPoint);
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
