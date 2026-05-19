using Assets.Scripts.Camera;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Crosshairs
{
    public interface ICrosshair
    {
        public CrosshairTypes CrosshairType { get; }

        public void SwitchMode(ViewType viewType);
        public void SetActive(bool isActive);
        public void SetPreviewPosition(Vector3 position);
    }
}
