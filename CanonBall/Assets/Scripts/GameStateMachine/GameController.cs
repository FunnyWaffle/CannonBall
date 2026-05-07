using Assets.Scripts.Config;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
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
            if (input.IsInteractionPerformed)
            {
                if (CameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                {
                    if (_interactionObjectsRepositiory.TryGetControllers(collider, out var cannonController))
                        _gameplayController.SetController(cannonController);
                    else if (_interactionObjectsRepositiory.TryGetUIWindow(collider, out var uIWindow))
                        _uIController.Open(uIWindow);
                }
            }

            if (!_uIController.HasOpenWindow)
                _gameplayController.HandleInput(input);

            _uIController.HandleInput(input);
        }
    }
}
