using UnityEngine;

namespace Assets.Scripts.Camera
{
    public interface ICameraTransformPreset
    {
        public Transform Pivot { get; }
        public Transform Slot { get; }
    }
}
