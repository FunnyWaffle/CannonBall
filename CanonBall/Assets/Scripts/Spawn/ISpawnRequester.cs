using Assets.Scripts.Shop;
using System;
using UnityEngine;

namespace Assets.Scripts.Spawn
{
    public interface ISpawnRequester<T>
    {
        public event EventHandler<SpawnArguments> SpawnRequested;

        public void SetSpawnedObject(T obj);
    }

    public class SpawnArguments
    {
        public ItemTypes ItemType { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Transform Parent { get; }

        public SpawnArguments(
            ItemTypes itemType,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            ItemType = itemType;
            Position = position;
            Rotation = rotation;
            Parent = parent;
        }
    }
}
