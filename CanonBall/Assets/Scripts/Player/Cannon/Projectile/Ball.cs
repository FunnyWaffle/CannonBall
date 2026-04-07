using UnityEngine;

namespace Assets.Scripts.Player.Cannon.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float _startPower;
        [SerializeField] private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            //_rigidbody.AddForce(new Vector3(0f, 0f, _startPower), ForceMode.Impulse);

            _rigidbody.linearVelocity = transform.forward * _startPower;
        }
    }
}
