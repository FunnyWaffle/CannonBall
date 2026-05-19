using Assets.Scripts.Camera;
using Assets.Scripts.Crosshairs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class CrosshairSystem
    {
        private readonly CameraSystem _cameraSystem;

        private readonly Dictionary<CrosshairTypes, ICrosshair> _crosshairs = new();

        private ICrosshair _crosshair;

        public CrosshairSystem(CameraSystem cameraSystem, params ICrosshair[] crosshairs)
        {
            _cameraSystem = cameraSystem;

            foreach (var crosshair in crosshairs)
            {
                _crosshairs[crosshair.CrosshairType] = crosshair;
            }

            _crosshair = _crosshairs[CrosshairTypes.Player];
        }

        public void EnableCrosshair(CrosshairTypes crosshairType)
        {
            _crosshair.SetActive(false);
            _crosshair = _crosshairs[crosshairType];
            _crosshair.SetActive(true);
        }

        public void SetCrosshairPosition(Vector3 position)
        {
            _crosshair.SetPreviewPosition(position);
        }

        public void SwitchCrosshairMode(ViewType viewType)
        {
            _crosshair.SwitchMode(viewType);
        }
    }

    public enum CrosshairTypes
    {
        Player,
        Cannon
    }
}
