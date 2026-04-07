using Assets.Scripts.Player.Cannon.Projectile;
using Assets.Scripts.Spawn;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player.Cannon
{
    public class CannonShoot : MonoBehaviour
    {
        [SerializeField] private Ball _projectile;
        [SerializeField] private float _projectileRadius;
        [SerializeField] private Transform _barrelExit;

        [Inject] private Spawner _spawner;

        private void Start()
        {
            PlayerInput.Enable();

            PlayerInput.AttackActionPerformed += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            _spawner.Spawn(_projectile, _barrelExit.position + _projectileRadius * _barrelExit.forward, transform.rotation);
        }
    }
}