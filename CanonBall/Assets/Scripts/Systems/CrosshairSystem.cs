using Assets.Scripts.Crosshairs;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class CrosshairSystem
    {
        private readonly PlayerCrosshair _playerCrosshair;
        private readonly CannonCrosshair _cannonCrosshair;

        private CrosshairTypes _currentCrosshairType;

        public CrosshairSystem(PlayerCrosshair playerCrosshair,
            CannonCrosshair cannonCrosshair)
        {
            _playerCrosshair = playerCrosshair;
            _cannonCrosshair = cannonCrosshair;
        }

        public void EnableCrosshair(CrosshairTypes crosshairType)
        {
            _currentCrosshairType = crosshairType;

            switch (crosshairType)
            {
                case CrosshairTypes.Player:
                    _playerCrosshair.SetActive(true);
                    _cannonCrosshair.SetActive(false);
                    break;
                case CrosshairTypes.Cannon:
                    _playerCrosshair.SetActive(false);
                    _cannonCrosshair.SetActive(true);
                    break;
            }
        }

        public void SwitchCrosshairMode(int viewModeIndex)
        {
            if (_currentCrosshairType != CrosshairTypes.Cannon)
                return;

            var crosshairMode = viewModeIndex switch
            {
                0 => CrosshairMode.FirstPerson,
                1 => CrosshairMode.ThirdPerson,
            };
            _cannonCrosshair.SwitchMode(crosshairMode);
        }

        public void SetPositionToCrosshairPreview(Vector3 position)
        {

        }
    }

    public enum CrosshairTypes
    {
        Player,
        Cannon
    }
}
