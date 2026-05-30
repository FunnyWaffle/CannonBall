using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Creations.Player.Components
{
    public class PlayerAvatarMover
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

        public Vector3 GetVelocity()
        {
            return new Vector3(_horizontalVelocity.x, _verticalSpeed, _horizontalVelocity.z);
        }

        public void UpdateHorizontalVelocity(Vector2 input)
        {
            var mainCamera = _cameraSystem.MainCamera;

            var flatForward = Vector3.Normalize(Vector3.ProjectOnPlane(mainCamera.Forward, Vector3.up));
            var flatRight = Vector3.Normalize(Vector3.ProjectOnPlane(mainCamera.Right, Vector3.up));

            var direction = flatForward * input.y + flatRight * input.x;

            var targetVelocity = direction * Speed;

            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, 1);
            _horizontalVelocity = Vector3.ClampMagnitude(_horizontalVelocity, MaxVelocity);
        }

        public void UpdateVerticalSpeed(bool isGrounded)
        {
            if (!isGrounded)
                _verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }

        public void ApplyJumpToVelocity(bool isGrounded)
        {
            if (!isGrounded)
                return;

            _verticalSpeed = JumpPower;
        }

        public void Stop()
        {
            _horizontalVelocity = Vector3.zero;
        }
    }
}
