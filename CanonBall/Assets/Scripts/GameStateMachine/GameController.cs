using Assets.Scripts.Camera;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;

namespace Assets.Scripts.GameStateMachine
{
    public class GameController
    {
        private readonly PlayerAvatarInputProvider _gameplayController;
        private readonly UIController _uIController;

        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly CrosshairSystem _crosshairSystem;
        private readonly CameraSystem _cameraSystem;
        private readonly PlayerAvatarInput _gameplayInput;

        public GameController(PlayerAvatarInputProvider gameplayController,
            UIController uIController,
            PlaceObjectSystem placeObjectSystem,
            CrosshairSystem crosshairSystem,
            CameraSystem cameraSystem,
            PlayerAvatarInput gameplayInput)
        {
            _gameplayController = gameplayController;
            _uIController = uIController;
            _placeObjectSystem = placeObjectSystem;
            _crosshairSystem = crosshairSystem;
            _cameraSystem = cameraSystem;
            _gameplayInput = gameplayInput;

            _gameplayInput.AttackActionPerformed += OnAttackPerform;
            _gameplayInput.ViewModeActionPerformed += OnViewModeActionPerform;
            _gameplayInput.BackActionPerformed += OnBackActionPerform;
            _gameplayInput.InventoryActionPerformed += OnInventoryActionPerform;
        }

        private void OnAttackPerform()
        {
            if (_uIController.OpenWindow.HasValue)
                return;

            if (_placeObjectSystem.IsPlacingObject)
                _placeObjectSystem.Place();
            else
                _gameplayController.OnAttack();
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

        private void OnBackActionPerform()
        {
            _uIController.ClearOpenWindow();
        }

        private void OnInventoryActionPerform()
        {
            var openWindow = _uIController.OpenWindow;
            if (!openWindow.HasValue && openWindow != UIWindowTypes.Inventory)
                _uIController.Open(UIWindowTypes.Inventory);
            else
                _uIController.ClearOpenWindow();
        }
    }
}
