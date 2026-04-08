using Assets.Scripts.Player.Cannon.Projectile;
using Assets.Scripts.Spawn;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player.Cannon
{
    public class CannonShoot : MonoBehaviour
    {
        [SerializeField] private Ball _projectile;
        [SerializeField] private Transform _barrelExit;

        [Inject] private Spawner _spawner;
        [Inject] private PlayerInput _input;

        private void Start()
        {
            _input.AttackActionPerformed += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            var ball = _spawner.Spawn(_projectile,
                _barrelExit.position + _projectile.Radius * _barrelExit.forward,
                _barrelExit.rotation);

            ball.MakeFly();
        }
    }
}