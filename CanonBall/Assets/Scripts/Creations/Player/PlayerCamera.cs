using Assets.Scripts.Wrappers;
using System;
using UnityEngine;

namespace Assets.Scripts.Creations.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private SerializableDictionary<CameraViewType, CameraTransformPreset> _cameraViewPresets;

        private Transform _cameraTransform;
        private Transform _currentPivot;

        public Vector3 Position => _cameraTransform.position;
        public Vector3 Forward => _cameraTransform.forward;
        public Vector3 PivotEulerAngles => _camera.transform.parent.eulerAngles;

        public void SetPivotLocalRotation(Quaternion rotation)
        {
            _currentPivot.localRotation = rotation;
        }

        public Vector3 WorldToScreenPoint(Vector3 position)
        {
            return _camera.WorldToScreenPoint(position);
        }

        public void SetViewMode(int viewModeIndex)
        {
            var cameraTransformPreset = viewModeIndex switch
            {
                0 => _cameraViewPresets[CameraViewType.FirstPerson],
                1 => _cameraViewPresets[CameraViewType.ThirdPerson],
                _ => throw new NotImplementedException(),
            };

            _currentPivot = cameraTransformPreset.Parent;
            _currentPivot.localRotation = Quaternion.identity;

            _cameraTransform.SetParent(cameraTransformPreset.Parent, false);
            _cameraTransform.position = cameraTransformPreset.Position;
        }

        public Vector3 GetFacedPosition()
        {
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, float.PositiveInfinity))
                return hit.point;
            else
                return _cameraTransform.position + _cameraTransform.forward * 10f;
        }

        private void Awake()
        {
            SetStartValues();
        }

        private void SetStartValues()
        {
            _cameraTransform = _camera.transform;
            SetViewMode(0);
        }

        [Serializable]
        private class CameraTransformPreset
        {
            [SerializeField] private Transform _parent;
            [SerializeField] private Transform _position;

            public Transform Parent => _parent;
            public Vector3 Position => _position.position;
        }

        private enum CameraViewType
        {
            Default,
            FirstPerson,
            ThirdPerson,
        }
    }
}