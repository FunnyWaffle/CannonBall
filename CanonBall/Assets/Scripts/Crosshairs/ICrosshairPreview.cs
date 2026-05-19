using Assets.Scripts.Camera;
using UnityEngine;

namespace Assets.Scripts.Crosshairs
{
    public interface ICrosshairPreview
    {
        public ViewType ViewType { get; }

        public void SetPosition(Vector3 position);
        public void SetActive(bool isActive);
    }
}
