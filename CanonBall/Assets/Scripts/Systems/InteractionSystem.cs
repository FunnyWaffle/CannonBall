using Assets.Scripts.Config;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Systems
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] private RectTransform _interactionPrompt;

        [Inject] private CameraSystem _cameraSystem;

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
    }
}
