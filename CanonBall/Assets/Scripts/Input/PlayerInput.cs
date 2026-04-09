using System;
using UnityEngine;

public class PlayerInput
{
    private readonly InputSystem_Actions _actions = new();

    public Vector2 Movement => _actions.Player.Move.ReadValue<Vector2>();
    public Vector2 Look => _actions.Player.Look.ReadValue<Vector2>();

    public event Action JumpActionPerformed;
    public event Action AttackActionPerformed;
    public event Action<int> ViewModeActionPerformed;

    public PlayerInput()
    {
        _actions.Enable();

        _actions.Player.Jump.performed += context => JumpActionPerformed?.Invoke();
        _actions.Player.Attack.performed += context => AttackActionPerformed?.Invoke();
        _actions.Player.FirstPersonView.performed += context => ViewModeActionPerformed?.Invoke(0);
        _actions.Player.ThirdPersonView.performed += context => ViewModeActionPerformed?.Invoke(1);
    }
}
