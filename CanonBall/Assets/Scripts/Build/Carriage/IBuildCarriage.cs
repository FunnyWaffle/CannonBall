using Assets.Scripts.Creations;
using UnityEngine;

namespace Assets.Scripts.Build.Carriage
{
    public interface IBuildCarriage : IComponent
    {
        public float TrunkOffset { get; }
        public Vector3 TrunkExit { get; }
        public Vector3 TrunkExitForward { get; }
    }
}
