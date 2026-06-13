using System;
using UnityEngine;

namespace Assets.Scripts.Creations
{
    public interface IRotationChangeNotifier : IComponent
    {
        public event Action<Quaternion> RotationChanged;
    }
}
