using Assets.Scripts.Input;
using Assets.Scripts.Placement;

namespace Assets.Scripts.GameStateMachine
{
    public class PlayerAvatarAttackInputProvider
    {
        private readonly PlayerAvatarInput _input;
        private readonly CurrentPlayerAvatarController _controllerPlaceholder;
        private readonly PlaceObjectSystem _placeObjectSystem;

        public PlayerAvatarAttackInputProvider(
            PlayerAvatarInput input,
            CurrentPlayerAvatarController controllerPlaceholder,
            PlaceObjectSystem placeObjectSystem)
        {
            _input = input;
            _controllerPlaceholder = controllerPlaceholder;
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
            var controller = _controllerPlaceholder.GetController();
            controller.Attack();
        }

        private void ExecutePlaceObject()
        {
            _placeObjectSystem.Place();
        }
    }
}
