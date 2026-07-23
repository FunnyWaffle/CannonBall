using Assets.Scripts.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraPresetHandler
    {
        private readonly SerializableDictionary<ViewType, CameraTransformPreset> _cameraViewPresets;

        private CameraTransformPreset _currentPreset;
        public ICameraTransformPreset CurrentPreset => _currentPreset;

        public CameraPresetHandler(SerializableDictionary<ViewType, CameraTransformPreset> cameraViewPresets)
        {
            _cameraViewPresets = cameraViewPresets;

            _currentPreset = _cameraViewPresets[ViewType.FirstPerson];
            CurrentPreset.Pivot.gameObject.SetActive(true);
            CurrentPreset.Pivot.localRotation = Quaternion.identity;
        }

        public void SetViewType(ViewType cameraViewType)
        {
            var rotation = CurrentPreset.Pivot.rotation;
            CurrentPreset.Pivot.gameObject.SetActive(false);

            _currentPreset = _cameraViewPresets[cameraViewType];

            CurrentPreset.Pivot.gameObject.SetActive(true);
            CurrentPreset.Pivot.rotation = rotation;
        }

        public void SetCurrentPresetRotation(Quaternion rotation)
        {
            var angles = rotation.eulerAngles;
            var yaw = new Vector3(0, angles.y, 0);

            var preset = _currentPreset;
            preset.SetPivotRotation(Quaternion.Euler(yaw));
            preset.SetSlotRotation(rotation);
        }

        public ICameraTransformPreset GetPreset(ViewType cameraViewType)
        {
            return _cameraViewPresets[cameraViewType];
        }
    }
}
