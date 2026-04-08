using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PlayerAimer : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _sensitivity = 10f;

        [Inject] private PlayerInput _playerInput;

        private readonly float _verticalEdge = 70;
        private Vector2 _rotation;


        private void Start()
        {
            _rotation = _camera.transform.eulerAngles;
        }

        private void Update()
        {
            var lookInput = _playerInput.Look;

            var newRotation = _sensitivity * Time.deltaTime * new Vector2(-lookInput.y, lookInput.x);

            _rotation = new Vector2(
                Mathf.Clamp(_rotation.x + newRotation.x, -_verticalEdge, _verticalEdge),
                _rotation.y + newRotation.y);
            _camera.rotation = Quaternion.Euler(_rotation);
        }
    }
}