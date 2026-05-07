using R3;
using UnityEngine;

namespace Assets.Scripts.Creations.Player.Components
{
    public class Aimer
    {
        private readonly float _sensitivity;
        private readonly float _verticalEdge = 70;

        private Vector2 _eulerRotation;
        private readonly ReactiveProperty<Quaternion> _rotation = new();

        public Aimer(float sensitivity, Vector2 startEulers)
        {
            _sensitivity = sensitivity;
            _eulerRotation = startEulers;
        }

        public ReadOnlyReactiveProperty<Quaternion> Rotation => _rotation;

        public void Aim(Vector2 input)
        {
            var newRotation = _sensitivity * Time.deltaTime * new Vector2(-input.y, input.x);

            _eulerRotation = new Vector2(
                Mathf.Clamp(_eulerRotation.x + newRotation.x, -_verticalEdge, _verticalEdge),
                _eulerRotation.y + newRotation.y);

            _rotation.Value = Quaternion.Euler(_eulerRotation);
        }
    }
}