using Assets.Scripts.Config;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.GameStateMachine.CannonControl;
using Assets.Scripts.GameStateMachine.PlayerControl;
using Assets.Scripts.Input;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Interaction
{
    public class PlayerInteractionSystem : MonoBehaviour
    {
        [SerializeField] private RectTransform _interactionPrompt;

        [SerializeField] private float _maxInteractionDistance = 1f;

        private PlayerAvatarInput _playerInput;
        [Inject] private CameraSystem _cameraSystem;
        [Inject] private InteractionObjectsRepositiory _interactionObjectsRepositiory;
        [Inject] private World _world;
        [Inject] private PlayerAvatarMovementInputProvider _playerAvatarInputProvider;
        [Inject] private ActiveCannonControllerContainer _activeCannonControllerContainer;
        [Inject] private UIController _uIController;
        [Inject] private InputSystem _inputSystem;
        [Inject] private ActivePlayerAvatarControllerContainer _currentPlayerAvatarController;

        [Inject]
        public void Initialize(PlayerAvatarInput playerInput)
        {
            _playerInput = playerInput;

            _playerInput.InteractionActionPerformed += OnInteractionPerform;
        }

        private void Update()
        {
            if (_cameraSystem.TryGetMainCameraFacedCollider(out var collider, _maxInteractionDistance, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
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
            if (!_cameraSystem.TryGetMainCameraFacedCollider(out var collider, _maxInteractionDistance, LayerIds.BitMaskPlayer | LayerIds.BitMaskGround))
                return;

            if (_world.SpatialObjectsMap.TryGetValue(collider, out var spatialObject)
                && _world.EntityComponents.TryGetValue(spatialObject, out var components)
                && components.TryGet<ICannonController>(out var controller))
            {
                _currentPlayerAvatarController.ClearController();
                _activeCannonControllerContainer.SetController(controller);
                _inputSystem.SwitchTo(InputType.Cannon);
            }
            else if (_interactionObjectsRepositiory.TryGetUIWindow(collider, out var uIWindow))
            {
                if (uIWindow is ShopView shopView
                    && _interactionObjectsRepositiory.TryGetItemSeller(collider, out var itemSeller))
                    _ = shopView.SetItemSeller(itemSeller);

                _inputSystem.SwitchTo(InputType.Shop);
                _uIController.Open(uIWindow);
            }
        }
    }
}
