using System;

namespace Assets.Scripts.Spawn.Factories
{
    public interface ISpawnRequesterCreator<T>
    {
        public event Action<ISpawnRequester<T>> SpawnRequesterCreated;
    }
}
