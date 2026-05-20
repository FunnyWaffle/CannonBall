using Assets.Scripts.Camera;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class GameController : IUpdatable
    {
        private readonly GameplayController _gameplayController;
        private readonly UIController _uIController;

        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly CrosshairSystem _crosshairSystem;
        private readonly CameraSystem _cameraSystem;
        private readonly GameplayInput _gameplayInput;
        private readonly Aimer _aimer;

        public GameController(GameplayController gameplayController,
            UIController uIController,
            PlaceObjectSystem placeObjectSystem,
            CrosshairSystem crosshairSystem,
            CameraSystem cameraSystem,
            GameplayInput gameplayInput,
            Aimer aimer)
        {
            _gameplayController = gameplayController;
            _uIController = uIController;
            _placeObjectSystem = placeObjectSystem;
            _crosshairSystem = crosshairSystem;
            _cameraSystem = cameraSystem;
            _gameplayInput = gameplayInput;
            _aimer = aimer;

            _gameplayInput.AttackActionPerformed += OnAttackPerform;
            _gameplayInput.ViewModeActionPerformed += OnViewModeActionPerform;
            _gameplayInput.BackActionPerformed += OnBackActionPerform;
            _gameplayInput.InventoryActionPerformed += OnInventoryActionPerform;
        }

        public void Update()
        {
            if (!_uIController.OpenWindow.HasValue)
            {
                var rotation = _aimer.Aim(_gameplayInput.Look);

                _cameraSystem.RotateCameraPivot(rotation);
                var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore);

                _gameplayController.HandleRotation(position);
                _gameplayController.HandleMovement(_gameplayInput.Movement);
            }
        }

        private void OnAttackPerform()
        {
            if (_uIController.OpenWindow.HasValue)
                return;

            if (_placeObjectSystem.IsPlacingObject)
                _placeObjectSystem.Place();
            else
                _gameplayController.HandleAttack();
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
