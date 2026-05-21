using Assets.Scripts.Input;
using Assets.Scripts.Placement;

namespace Assets.Scripts.GameStateMachine
{
    public class PlayerAvatarAttackInputProvider
    {
        private readonly PlayerAvatarInput _input;
        private readonly CurrentPlayerAvatarController _currentController;
        private readonly PlaceObjectSystem _placeObjectSystem;

        public PlayerAvatarAttackInputProvider(
            PlayerAvatarInput input,
            CurrentPlayerAvatarController currentController,
            PlaceObjectSystem placeObjectSystem)
        {
            _input = input;
            _currentController = currentController;
            _placeObjectSystem = placeObjectSystem;

            _input.AttackActionPerformed += OnAttack;
        }

        public void OnAttack()
        {
            if (_placeObjectSystem.IsPlacingObject)
                ExecutePlaceObject();
            else
                ExecuteControllerAttack();
        }

        private void ExecuteControllerAttack()
        {
            if (!_currentController.TryGetController(out var controller))
                return;

            controller.Attack();
        }

        private void ExecutePlaceObject()
        {
            _placeObjectSystem.Place();
        }
    }
}
