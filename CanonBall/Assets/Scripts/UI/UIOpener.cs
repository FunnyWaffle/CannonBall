using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;

namespace Assets.Scripts.UI
{
    public class UIOpener
    {
        private readonly PlayerAvatarInput _playerInput;
        private readonly UIInput _uIInput;
        private readonly UIController _uIController;
        private readonly InputSystem _inputSystem;

        public UIOpener(PlayerAvatarInput playerInput, UIInput uIInput, UIController uIController, InputSystem inputSystem)
        {
            _playerInput = playerInput;
            _uIInput = uIInput;
            _uIController = uIController;
            _inputSystem = inputSystem;

            _playerInput.InventoryActionPerformed += OnInventoryActionPerform;

            _uIInput.CancelPefromed += OnCancel;
        }

        private void OnInventoryActionPerform()
        {
            var openWindow = _uIController.OpenWindow;
            if (!openWindow.HasValue && openWindow != UIWindowTypes.Inventory)
            {
                _uIController.Open(UIWindowTypes.Inventory);
                _inputSystem.SwitchTo(InputType.UI);
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
        }
    }
}
