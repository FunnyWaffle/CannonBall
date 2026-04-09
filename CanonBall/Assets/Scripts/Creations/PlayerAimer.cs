using UnityEngine;

namespace Assets.Scripts.Creations
{
    public class PlayerAimer : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 10f;

        private readonly float _verticalEdge = 70;
        private Vector2 _rotation;

        public void SetStartEulers(Vector2 eulers)
        {
            _rotation = eulers;
        }

        public Quaternion GetAimRotation(Vector2 input)
        {
            var newRotation = _sensitivity * Time.deltaTime * new Vector2(-input.y, input.x);

            _rotation = new Vector2(
                Mathf.Clamp(_rotation.x + newRotation.x, -_verticalEdge, _verticalEdge),
                _rotation.y + newRotation.y);

            return Quaternion.Euler(_rotation);
        }
    }
}