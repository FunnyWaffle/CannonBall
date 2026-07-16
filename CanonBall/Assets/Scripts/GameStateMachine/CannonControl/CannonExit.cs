using Assets.Scripts.GameStateMachine.PlayerControl;
using Assets.Scripts.Input;

namespace Assets.Scripts.GameStateMachine.CannonControl
{
    public class CannonExit
    {
        private readonly CannonInput _cannonInput;
        private readonly ActiveCannonControllerContainer _controllerContainer;
        private readonly ActivePlayerAvatarControllerContainer _playerAvatarController;
        private readonly InputSystem _inputSystem;

        public CannonExit(
            CannonInput cannonInput,
            ActiveCannonControllerContainer controllerContainer,
            ActivePlayerAvatarControllerContainer playerAvatarController,
            InputSystem inputSystem)
        {
            _cannonInput = cannonInput;
            _controllerContainer = controllerContainer;
            _playerAvatarController = playerAvatarController;
            _inputSystem = inputSystem;

            _cannonInput.ExitPerform += OnExit;
        }

        private void OnExit()
        {
            _controllerContainer.ClearController();
            _playerAvatarController.SetLastController();
            _inputSystem.SwitchToLast();
        }
    }
}
