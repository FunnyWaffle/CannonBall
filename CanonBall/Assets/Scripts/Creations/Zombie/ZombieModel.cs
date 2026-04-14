using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieModel : MonoBehaviour
    {
        [SerializeField] private Transform _model;

        public Vector3 Position => _model.position;

        public void SetPosition(Vector3 position)
        {
            if (Physics.Raycast(position, Vector3.down, out var hit, 5f))
            {
                Vector3 pos = _model.position;
                pos.y = hit.point.y;

                _model.position = pos;
            }
        }
    }
}
