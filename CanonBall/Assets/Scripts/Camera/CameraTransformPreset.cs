using System;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    [Serializable]
    public class CameraTransformPreset : ICameraTransformPreset
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _slot;

        public Transform Pivot => _pivot;
        public Transform Slot => _slot;

        public void SetPivotRotation(Quaternion rotation)
        {
            Pivot.rotation = rotation;
        }

        public void SetSlotRotation(Quaternion rotation)
        {
            Slot.rotation = rotation;
        }
    }
}