using UnityEngine;

public class CannonMover : MonoBehaviour
{
    [SerializeField] private float _motorPower = 1000f;
    [SerializeField] private float _brakePower = 5000f;
    [SerializeField] private WheelCollider[] _wheels;

    private void Start()
    {
        PlayerInput.Enable();
    }

    private void Update()
    {
        var movementInput = PlayerInput.Movement;

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
