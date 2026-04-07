using UnityEngine;

namespace Assets.Scripts.Player.Cannon
{
    public class CannonShoot : MonoBehaviour
    {
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Vector3 _projectileSpawnPosition;

        private void Start()
        {
            PlayerInput.Enable();

            PlayerInput.AttackActionPerformed += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            Instantiate(_projectile, _projectileSpawnPosition, transform.rotation);
        }
    }
}