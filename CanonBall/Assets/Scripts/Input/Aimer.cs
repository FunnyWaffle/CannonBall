using Assets.Scripts.Config;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class Aimer
    {
        private readonly float _sensitivity;
        private readonly float _verticalEdge = 57;

        private Vector2 _eulerRotation;

        public Aimer(PlayerConfig playerConfig)
        {
            _sensitivity = playerConfig.Sensitivity;
        }

        public Quaternion Aim(Vector2 input)
        {
            var newRotation = _sensitivity * Time.deltaTime * new Vector2(-input.y, input.x);

            _eulerRotation = new Vector2(
                Mathf.Clamp(_eulerRotation.x + newRotation.x,
                -_verticalEdge, _verticalEdge),
                _eulerRotation.y + newRotation.y);

            return Quaternion.Euler(_eulerRotation);
        }
    }
}