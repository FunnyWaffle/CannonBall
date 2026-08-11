using UnityEngine;

namespace Assets.Scripts.Navigation.Obstacles
{
    public class NavObstacle : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [SerializeField] private Collider[] _parts;

        public Bounds Bounds => _bounds;
        public Collider[] Parts => _parts;
    }
}
