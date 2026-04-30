using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Player.Components
{
    public class PlayerAvatarMover
    {
        private readonly float _speed;

        private Vector3 _velocity;

        public PlayerAvatarMover(float speed)
        {
            _speed = speed;
        }

        public Vector3 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                VelocityChanged?.Invoke(value);
            }
        }

        public event Action<Vector3> VelocityChanged;

        public void UpdateVelocity(Vector2 input)
        {
            Velocity = new Vector3(input.x, 0, input.y) * _speed;
        }

        public Vector3 ProjectVelocityOn(Vector3 forward, Vector3 right)
        {
            var flatForward = Vector3.Normalize(Vector3.ProjectOnPlane(forward, Vector3.up));
            var flatRight = Vector3.Normalize(Vector3.ProjectOnPlane(right, Vector3.up));

            return flatForward * _velocity.z + flatRight * _velocity.x;
        }
    }
}
