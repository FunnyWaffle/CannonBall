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

        public GameController(GameplayController gameplayController,
            UIController uIController,
            InteractionObjectsRepositiory interactionObjectsRepositiory)
        {
            _gameplayController = gameplayController;
            _uIController = uIController;
            _interactionObjectsRepositiory = interactionObjectsRepositiory;
        }

        public void HandleInput(InputData input)
        {
            HandleInteraction(input.IsInteractionPerformed);

            if (!_uIController.HasOpenWindow)
                _gameplayController.HandleInput(input);

            _uIController.HandleInput(input);
        }

        private void HandleInteraction(bool isInteractionPerformed)
        {
            if (!isInteractionPerformed)
                return;

            if (CameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                return;

            if (_interactionObjectsRepositiory.TryGetControllers(collider, out var cannonController))
                _gameplayController.SetController(cannonController);

            else if (_interactionObjectsRepositiory.TryGetUIWindow(collider, out var uIWindow))
            {
                if (uIWindow is ShopView shopView
                    && _interactionObjectsRepositiory.TryGetItemSeller(collider, out var itemSeller))
                    shopView.SetItemSeller(itemSeller);

                _uIController.Open(uIWindow);
            }
        }
    }
}
