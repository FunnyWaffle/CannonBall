using System;
using UnityEngine;

namespace Assets.Scripts.Creations
{
    public interface IPositionChangeNotifier : IComponent
    {
        public event Action<Vector3> PositionChanged;
    }
}
