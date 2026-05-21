using Assets.Scripts.Camera;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;

namespace Assets.Scripts.GameStateMachine
{
    public class GameController
    {
        private readonly UIController _uIController;

        private readonly CrosshairSystem _crosshairSystem;
        private readonly CameraSystem _cameraSystem;
        private readonly PlayerAvatarInput _gameplayInput;

        public GameController(
            UIController uIController,
            CrosshairSystem crosshairSystem,
            CameraSystem cameraSystem,
            PlayerAvatarInput gameplayInput)
        {
            _uIController = uIController;
            _crosshairSystem = crosshairSystem;
            _cameraSystem = cameraSystem;
            _gameplayInput = gameplayInput;

            _gameplayInput.ViewModeActionPerformed += OnViewModeActionPerform;
        }

        private void OnViewModeActionPerform(int viewModeIndex)
        {
            if (!_uIController.OpenWindow.HasValue)
            {
                var viewType = viewModeIndex switch
                {
                    0 => ViewType.FirstPerson,
                    1 => ViewType.ThirdPerson,
                    _ => throw new System.NotImplementedException(),
                };
                _crosshairSystem.SwitchCrosshairMode(viewType);
                _cameraSystem.ChangeCameraViewType(viewType);
            }
        }
    }
}
