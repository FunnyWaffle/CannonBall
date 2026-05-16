using Assets.Scripts.Config;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Shop;
using Assets.Scripts.Systems;

namespace Assets.Scripts.GameStateMachine
{
    public class GameController
    {
        private readonly GameplayController _gameplayController;
        private readonly UIController _uIController;

        private readonly InteractionObjectsRepositiory _interactionObjectsRepositiory;
        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly CrosshairSystem _crosshairSystem;

        public GameController(GameplayController gameplayController,
            UIController uIController,
            InteractionObjectsRepositiory interactionObjectsRepositiory,
            PlaceObjectSystem placeObjectSystem,
            CrosshairSystem crosshairSystem)
        {
            _gameplayController = gameplayController;
            _uIController = uIController;
            _interactionObjectsRepositiory = interactionObjectsRepositiory;
            _placeObjectSystem = placeObjectSystem;
            _crosshairSystem = crosshairSystem;
        }

        public void HandleInput(InputData input)
        {
            HandleInteraction(input.IsInteractionPerformed);

            if (!_uIController.HasOpenWindow)
            {
                HadnleAttack(input.IsAttacked);
                HandleCameraModeSwitch(input.ViewModeIndex);
                _gameplayController.HandleInput(input);
            }

            _uIController.HandleInput(input);
        }

        private void HandleInteraction(bool isInteractionPerformed)
        {
            if (!isInteractionPerformed)
                return;

            if (!CameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
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

        private void HadnleAttack(bool isAttackPressed)
        {
            if (!isAttackPressed)
                return;

            _placeObjectSystem.Place();
        }

        private void HandleCameraModeSwitch(int viewModeIndex)
        {
            _crosshairSystem.SwitchCrosshairMode(viewModeIndex);
        }
    }
}
