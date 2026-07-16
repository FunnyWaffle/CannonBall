using Assets.Scripts.Creations;
using Unity.AI.Navigation;
using UnityEngine;

namespace Assets.Scripts.Navigation
{
    public class NotForVehicleZone : MonoBehaviour, IComponent
    {
        [SerializeField] private NavMeshModifierVolume _navMeshModifierVolume;

        public void AdjustSize(Vector3 size)
        {
            _navMeshModifierVolume.size = size;
        }
    }
}
