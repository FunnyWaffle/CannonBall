using System;

namespace Assets.Scripts.Player
{
    public class Player
    {
        private readonly PlayerAimer _aimer;
        private readonly PlayerCamera _camera;
        private readonly PlayerInput _input;

        private readonly Updater _updater;

        public Player(PlayerAimer playerAimer, PlayerCamera playerCamera, PlayerInput playerInput, Updater updater)
        {
            _aimer = playerAimer;
            _camera = playerCamera;
            _input = playerInput;

            _updater = updater;

            _aimer.SetStartEulers(_camera.PivotEulerAngles);

            _updater.UpdatePerformed += Update;
            _input.ViewModeActionPerformed += OnViewModeActionPerform;
        }

        public PlayerCamera Camera => _camera;

        public event Action<int> ViewModeChanged;

        private void Update()
        {
            var rotation = _aimer.GetAimRotation(_input.Look);
            _camera.SetPivotLocalRotation(rotation);
        }

        private void OnViewModeActionPerform(int viewModeIndex)
        {
            _camera.SetViewMode(viewModeIndex);
            ViewModeChanged?.Invoke(viewModeIndex);
        }
    }
}
