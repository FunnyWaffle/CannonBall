using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Creations.Player.Components
{
    public class PlayerAvatarMover : IComponent
    {
        private readonly CameraSystem _cameraSystem;

        private float _verticalSpeed;
        private Vector3 _horizontalVelocity;

        public PlayerAvatarMover(CameraSystem cameraSystem)
        {
            _cameraSystem = cameraSystem;
        }

        public float Speed { get; set; }
        public float JumpPower { get; set; }
        public float MaxVelocity { get; set; }
        public float MovementAcceleration { get; set; }
        public float MovementDeceleration { get; set; }
        public float MovementAirAcceleration { get; set; }
        public float MovementAirDeceleration { get; set; }

        public Vector3 GetVelocity()
        {
            return new Vector3(_horizontalVelocity.x, _verticalSpeed, _horizontalVelocity.z);
        }

        public void UpdateHorizontalVelocity(Vector2 input, bool isGrounded)
        {
            var mainCamera = _cameraSystem.MainCamera;

            var flatForward = Vector3.Normalize(Vector3.ProjectOnPlane(mainCamera.Forward, Vector3.up));
            var flatRight = Vector3.Normalize(Vector3.ProjectOnPlane(mainCamera.Right, Vector3.up));

            var direction = flatForward * input.y + flatRight * input.x;

            var targetVelocity = direction * Speed;

            var speedDelta = GetSpeedDelta(input == Vector2.zero, isGrounded);

            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, speedDelta);
            _horizontalVelocity = Vector3.ClampMagnitude(_horizontalVelocity, MaxVelocity);
        }

        public void UpdateVerticalSpeed(bool isGrounded)
        {
            if (!isGrounded)
                _verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }

        public bool ApplyJumpToVelocity(bool isGrounded)
        {
            if (!isGrounded)
                return false;

            _verticalSpeed = JumpPower;
            return true;
        }

        public void Stop()
        {
            _horizontalVelocity = Vector3.zero;
        }

        private float GetSpeedDelta(bool isMoving, bool isGrounded)
        {
            if (!isGrounded)
                return isMoving ? MovementAirAcceleration : MovementAirDeceleration;

            return isMoving ? MovementAcceleration : MovementDeceleration;
        }
    }
}
