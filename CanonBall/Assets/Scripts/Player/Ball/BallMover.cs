using UnityEngine;

public class BallMover : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _velocity = 100f;
    [SerializeField] private float _jumpPower = 5f;

    private void Start()
    {
        PlayerInput.Enable();
        PlayerInput.JumpActionPerformed += HandleJumpAction;
    }

    private void FixedUpdate()
    {
        Vector2 movementInput = PlayerInput.Movement;
        _rigidbody.linearVelocity += _velocity * Time.fixedDeltaTime * new Vector3(movementInput.x, 0f, movementInput.y);
    }

    private void HandleJumpAction()
    {
        _rigidbody.AddForce(_jumpPower * Vector3.up, ForceMode.Force);
    }
}
