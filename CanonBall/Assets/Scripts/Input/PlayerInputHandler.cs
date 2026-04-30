using Assets.Scripts.Creations;

namespace Assets.Scripts.Input
{
    public class PlayerInputHandler : IUpdatable
    {
        private readonly PlayerInput _input;
        private readonly GameStateMachine.GameStateMachine _gameStateMachine;

        private int _viewModeIndex;
        private bool _isInteractionPerformed;
        private bool _isAttacked;
        private bool _isBackEventPerformed;
        private bool _isInventoryEventPerformed;

        public PlayerInputHandler(PlayerInput playerInput,
            GameStateMachine.GameStateMachine gameStateMachine)
        {
            _input = playerInput;
            _gameStateMachine = gameStateMachine;

            _input.ViewModeActionPerformed += OnViewModeChange;
            _input.InteractionActionPerformed += OnInteractionPerform;
            _input.AttackActionPerformed += OnAttackPerform;
            _input.BackActionPerformed += OnBackPerform;
            _input.InventoryActionPerformed += OnInventoryActionPerform;
        }

        public void Update()
        {
            var input = new InputData(
                _input.Movement,
                _input.Look,
                _viewModeIndex,
                _isInteractionPerformed,
                _isAttacked,
                _isBackEventPerformed,
                _isInventoryEventPerformed);

            _isInteractionPerformed = false;
            _isAttacked = false;
            _isBackEventPerformed = false;
            _isInventoryEventPerformed = false;

            _gameStateMachine.HandleInput(input);
        }

        private void OnBackPerform() => _isBackEventPerformed = true;

        private void OnInventoryActionPerform() => _isInventoryEventPerformed = true;

        private void OnViewModeChange(int index) => _viewModeIndex = index;

        private void OnInteractionPerform() => _isInteractionPerformed = true;

        private void OnAttackPerform() => _isAttacked = true;
    }
}
