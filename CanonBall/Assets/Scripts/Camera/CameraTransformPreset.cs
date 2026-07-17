using System;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    [Serializable]
    public class CameraTransformPreset
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _position;

        public Transform Pivot => _pivot;
        public Transform Slot => _position;
        public Vector3 Position => _position.position;
    }
}