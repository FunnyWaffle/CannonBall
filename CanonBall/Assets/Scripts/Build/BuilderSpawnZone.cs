using UnityEngine;

namespace Assets.Scripts.Build
{
    public class BuilderSpawnZone : MonoBehaviour
    {
        [SerializeField] private Transform _point;

        public bool CanAccommodate()
        {
            return true;
        }

        public Vector3 GetPosition()
        {
            return _point.position;
        }
    }
}
