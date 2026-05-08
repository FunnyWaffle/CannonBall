using System;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public interface IFactory
    {
        public Type CreatedType { get; }
        public IPoolableObject Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
