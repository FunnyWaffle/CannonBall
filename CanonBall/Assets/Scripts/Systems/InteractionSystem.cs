using Assets.Scripts.Config;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Interaction;
using Assets.Scripts.Shop;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Systems
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] private RectTransform _interactionPrompt;

        private GameplayInput _gameplayInput;
        [Inject] private CameraSystem _cameraSystem;
        [Inject] private InteractionObjectsRepositiory _interactionObjectsRepositiory;
        [Inject] private GameplayController _gameplayController;
        [Inject] private UIController _uIController;


        [Inject]
        public void Initialize(GameplayInput gameplayInput)
        {
            _gameplayInput = gameplayInput;

            _gameplayInput.InteractionActionPerformed += OnInteractionPerform;
        }

        private void Update()
        {
            if (_cameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
            {
                var gameObject = collider.gameObject;
                var layer = gameObject.layer;

                if (layer == LayerIds.IndexVendor
                    || layer == LayerIds.IndexGun)
                {
                    _interactionPrompt.gameObject.SetActive(true);
                }
            }
            else
            {
                _interactionPrompt.gameObject.SetActive(false);
            }
        }

        private void OnInteractionPerform()
        {
            if (!_cameraSystem.TryGetMainCameraFacedCollider(out var collider, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                return;

            if (_interactionObjectsRepositiory.TryGetControllers(collider, out var cannonController))
            {
                _gameplayController.SetController(cannonController);
            }

            else if (_interactionObjectsRepositiory.TryGetUIWindow(collider, out var uIWindow))
            {
                if (uIWindow is ShopView shopView
                    && _interactionObjectsRepositiory.TryGetItemSeller(collider, out var itemSeller))
                    _ = shopView.SetItemSeller(itemSeller);

                _uIController.Open(uIWindow);
            }
        }
    }
}
