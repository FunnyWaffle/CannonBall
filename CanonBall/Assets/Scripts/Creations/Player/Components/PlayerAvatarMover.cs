using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Creations.Player.Components
{
    public class PlayerAvatarMover
    {
        private readonly CameraSystem _cameraSystem;

        private float _speed;
        private Vector3 _velocity;

        public PlayerAvatarMover(CameraSystem cameraSystem)
        {
            _cameraSystem = cameraSystem;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public Vector3 UpdateVelocity(Vector2 input)
        {
            _velocity = new Vector3(input.x, 0, input.y) * _speed;

            var mainCamera = _cameraSystem.MainCamera;
            var flatForward = Vector3.Normalize(Vector3.ProjectOnPlane(mainCamera.Forward, Vector3.up));
            var flatRight = Vector3.Normalize(Vector3.ProjectOnPlane(mainCamera.Right, Vector3.up));

            return flatForward * _velocity.z + flatRight * _velocity.x;
        }

        public void Stop()
        {
            _velocity = Vector3.zero;
        }
    }
}
