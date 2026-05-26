using Assets.Scripts.Combat;
using System.Collections.Generic;

namespace Assets.Scripts.Space
{
    public class SpatialObjectsMap
    {
        private readonly Dictionary<ISpatialObject, HitBox> _hitBoxes = new();

        public void Register(ISpatialObject spatialObject, HitBox hitBox)
        {
            _hitBoxes[spatialObject] = hitBox;
        }

        public bool TryGetHitBox(ISpatialObject spatialObject, out HitBox hitBox)
        {
            return _hitBoxes.TryGetValue(spatialObject, out hitBox);
        }
    }
}
