using Assets.Scripts.Camera;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Crosshairs
{
    public class PlayerCrosshair : MonoBehaviour, ICrosshair
    {
        public CrosshairTypes CrosshairType => CrosshairTypes.Player;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetPreviewPosition(Vector3 position) { }

        public void SwitchMode(ViewType viewType) { }
    }
}
