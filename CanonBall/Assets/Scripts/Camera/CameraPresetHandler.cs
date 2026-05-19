using Assets.Scripts.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraPresetHandler
    {
        private readonly SerializableDictionary<ViewType, CameraTransformPreset> _cameraViewPresets;

        public CameraTransformPreset CurrentPreset { get; private set; }

        public CameraPresetHandler(SerializableDictionary<ViewType, CameraTransformPreset> cameraViewPresets)
        {
            _cameraViewPresets = cameraViewPresets;

            CurrentPreset = _cameraViewPresets[ViewType.FirstPerson];
            CurrentPreset.Pivot.gameObject.SetActive(true);
            CurrentPreset.Pivot.localRotation = Quaternion.identity;
        }

        public void SetViewType(ViewType cameraViewType)
        {
            var rotation = CurrentPreset.Pivot.rotation;
            CurrentPreset.Pivot.gameObject.SetActive(false);

            CurrentPreset = _cameraViewPresets[cameraViewType];

            CurrentPreset.Pivot.gameObject.SetActive(true);
            CurrentPreset.Pivot.rotation = rotation;
        }

        public CameraTransformPreset GetPreset(ViewType cameraViewType)
        {
            return _cameraViewPresets[cameraViewType];
        }
    }
}
