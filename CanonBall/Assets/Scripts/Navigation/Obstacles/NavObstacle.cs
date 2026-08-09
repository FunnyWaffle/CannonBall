using UnityEngine;

namespace Assets.Scripts.Navigation.Obstacles
{
    public class NavObstacle : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;
        [SerializeField] private INavObstaclePart[] _parts;

        public Vector3 Center => transform.position;
        public Bounds Bounds => _bounds;
    }
}
