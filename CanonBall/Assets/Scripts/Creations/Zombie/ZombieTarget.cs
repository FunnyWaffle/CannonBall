using Assets.Scripts.Space;
using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieTarget
    {
        public ISpatialObject Target { get; private set; }
        public Vector3 LastTargetPosition { get; private set; }
        public Vector3 TargetPosition { get; private set; }
        public bool HasMoved
        {
            get
            {
                if (_newTarget)
                {
                    _newTarget = false;
                    return true;
                }

                return Target.Position != LastTargetPosition;
            }
        }

        private bool _newTarget;

        public void Set(ISpatialObject target)
        {
            Target = target;
            LastTargetPosition = target.Position;

            _newTarget = true;
        }

        public void Set(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
        }
    }
}
