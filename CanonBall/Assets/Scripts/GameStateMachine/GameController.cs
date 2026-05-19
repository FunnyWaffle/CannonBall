using Assets.Scripts.Camera;
using Assets.Scripts.Config;
using Assets.Scripts.Creations;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Shop;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.GameStateMachine
{
    public class GameController : IUpdatable
    {
        private readonly GameplayController _gameplayController;
        private readonly UIController _uIController;

        private readonly InteractionObjectsRepositiory _interactionObjectsRepositiory;
        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly CrosshairSystem _crosshairSystem;
        private readonly CameraSystem _cameraSystem;
        private readonly PlayerInput _playerInput;
        private readonly Aimer _aimer;

        public GameController(GameplayController gameplayController,
            UIController uIController,
            InteractionObjectsRepositiory interactionObjectsRepositiory,
            PlaceObjectSystem placeObjectSystem,
            CrosshairSystem crosshairSystem,
            CameraSystem cameraSystem,
            PlayerInput playerInput,
            Aimer aimer)
        {
            _gameplayController = gameplayController;
            _uIController = uIController;
            _interactionObjectsRepositiory = interactionObjectsRepositiory;
            _placeObjectSystem = placeObjectSystem;
            _crosshairSystem = crosshairSystem;
            _cameraSystem = cameraSystem;
            _playerInput = playerInput;
            _aimer = aimer;

            _playerInput.InteractionActionPerformed += OnInteractionPerform;
            _playerInput.AttackActionPerformed += OnAttackPerform;
            _playerInput.ViewModeActionPerformed += OnViewModeActionPerform;
            _playerInput.BackActionPerformed += OnBackActionPerform;
            _playerInput.InventoryActionPerformed += OnInventoryActionPerform;
        }

        public void Update()
        {
            if (!_uIController.OpenWindow.HasValue)
            {
                var rotation = _aimer.Aim(_playerInput.Look);

                _cameraSystem.RotateCameraPivot(rotation);
                var position = _cameraSystem.MainCamera.GetFacedPosition(QueryTriggerInteraction.Ignore);

                _gameplayController.HandleRotation(position);
                _gameplayController.HandleMovement(_playerInput.Movement);
            }
        }

        private void OnInteractionPerform()
        {
            if (!_cameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                return;

            if (_interactionObjectsRepositiory.TryGetControllers(collider, out var cannonController))
            {
                _gameplayController.SetController(cannonController);

                _crosshairSystem.EnableCrosshair(cannonController.CrosshairType);
            }

            else if (_interactionObjectsRepositiory.TryGetUIWindow(collider, out var uIWindow))
            {
                if (uIWindow is ShopView shopView
                    && _interactionObjectsRepositiory.TryGetItemSeller(collider, out var itemSeller))
                    _ = shopView.SetItemSeller(itemSeller);

                _uIController.Open(uIWindow);
            }
        }

        private void OnAttackPerform()
        {
            if (!_uIController.OpenWindow.HasValue)
            {
                _gameplayController.HandleAttack();
                _placeObjectSystem.Place();
            }
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
