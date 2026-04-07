using System;
using UnityEngine;

public static class PlayerInput
{
    private static InputSystem_Actions _actions = new();

    private static bool _isEnabled;

    public static Vector2 Movement => _actions.Player.Move.ReadValue<Vector2>();

    public static event Action JumpActionPerformed;
    public static event Action AttackActionPerformed;

    public static void Enable()
    {
        if (_isEnabled) return;

        _actions = new InputSystem_Actions();
        _actions.Enable();

        _isEnabled = true;

        _actions.Player.Jump.performed += (context) => { JumpActionPerformed?.Invoke(); };
        _actions.Player.Attack.performed += (context) => { AttackActionPerformed?.Invoke(); };
    }
}
