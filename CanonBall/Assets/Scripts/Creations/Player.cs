using Assets.Scripts.Guns;
using Assets.Scripts.Input;

namespace Assets.Scripts.Creations
{
    public class Player
    {
        private readonly PlayerAimer _aimer;
        private readonly PlayerCamera _camera;
        private readonly PlayerInput _input;
        private readonly Cannon _cannon;

        private readonly Updater _updater;

        public Player(PlayerAimer playerAimer,
            PlayerCamera playerCamera,
            PlayerInput playerInput,
            Updater updater,
            Cannon cannon)
        {
            _aimer = playerAimer;
            _camera = playerCamera;
            _input = playerInput;
            _cannon = cannon;

            _updater = updater;

            _aimer.SetStartEulers(_camera.PivotEulerAngles);

            _updater.UpdatePerformed += Update;

            _input.ViewModeActionPerformed += OnViewModeActionPerform;
            _input.AttackActionPerformed += OnAttackActionPerform;
        }

        private void Update()
        {
            var rotation = _aimer.GetAimRotation(_input.Look);
            _camera.SetPivotLocalRotation(rotation);

            _cannon.RotateToPosition(_camera.GetFacedPosition());
        }

        private void OnViewModeActionPerform(int viewModeIndex)
        {
            _camera.SetViewMode(viewModeIndex);
            _cannon.SetCrosshairType(viewModeIndex);
        }

        private void OnAttackActionPerform()
        {
            _cannon.Shoot();
        }
    }
}
