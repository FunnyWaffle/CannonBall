using Assets.Scripts.Input;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Guns
{
    public class CannonMover : MonoBehaviour
    {
        [SerializeField] private float _motorPower = 1;
        [SerializeField] private WheelCollider[] _wheels;

        [Inject] private PlayerInput _playerInput;

        private void Update()
        {
            var movementInput = _playerInput.Movement;

            if (movementInput.y != 0f)
                foreach (var wheel in _wheels)
                {
                    wheel.motorTorque = movementInput.y * _motorPower;
                }

            foreach (var wheel in _wheels)
            {
                wheel.steerAngle = movementInput.x * 10f;
            }
        }
    }
}