using Assets.Scripts.Input;

namespace Assets.Scripts.GameStateMachine
{
    public class InventoryInputProvider
    {
        private readonly PlayerAvatarInput _playerInput;
        private readonly CannonInput _cannonInput;
        private readonly InventoryInput _inventoryInput;
        private readonly UIController _uIController;
        private readonly InputSystem _inputSystem;

        public InventoryInputProvider(PlayerAvatarInput playerInput, CannonInput cannonInput, InventoryInput uIInput, UIController uIController, InputSystem inputSystem)
        {
            _playerInput = playerInput;
            _cannonInput = cannonInput;
            _inventoryInput = uIInput;
            _uIController = uIController;
            _inputSystem = inputSystem;

            _playerInput.InventoryActionPerformed += OnInventoryActionPerform;
            _cannonInput.InventoryActionPerformed += OnInventoryActionPerform;

            _inventoryInput.ClosePefromed += OnCancel;
        }

        private void OnInventoryActionPerform()
        {
            var openWindow = _uIController.OpenWindow;
            if (!openWindow.HasValue && openWindow != UIWindowTypes.Inventory)
            {
                _uIController.Open(UIWindowTypes.Inventory);
                _inputSystem.SwitchTo(InputType.Inventory);
            }
            else
            {
                _uIController.ClearOpenWindow();
                _inputSystem.SwitchToLast();
            }
        }

        private void OnCancel()
        {
            _uIController.ClearOpenWindow();
            _inputSystem.SwitchToLast();
        }
    }
}
