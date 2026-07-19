using Assets.Scripts.Input;

namespace Assets.Scripts.GameStateMachine
{
    public class ShopInputProvider
    {
        private readonly ShopInput _input;
        private readonly UIController _uIController;
        private readonly InputSystem _inputSystem;

        public ShopInputProvider(ShopInput input, UIController uIController, InputSystem inputSystem)
        {
            _input = input;
            _uIController = uIController;
            _inputSystem = inputSystem;

            _input.Closed += OnClose;
        }

        private void OnClose()
        {
            _uIController.ClearOpenWindow();
            _inputSystem.SwitchToLast();
        }
    }
}
