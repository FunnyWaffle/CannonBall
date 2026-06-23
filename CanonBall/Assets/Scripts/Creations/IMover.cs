using System;
using UnityEngine;

namespace Assets.Scripts.Creations
{
    public interface IMover : IComponent
    {
        public event Action Arrived;

        public void SetDestination(Vector3 position);
    }
}
