using Assets.Scripts.Spawn.Factories;
using System.Collections.Generic;

namespace Assets.Scripts.Spawn
{
    public class SpawnRequesterCreationHandler<T>
        where T : ISpawnable, IPoolableObject
    {
        private readonly Spawner<T> _spawner;
        private readonly ISpawnRequesterCreator<T> _creator;
        private readonly List<SpawnRequestHandler<T>> _requesters;

        public SpawnRequesterCreationHandler(Spawner<T> spawner, ISpawnRequesterCreator<T> creator)
        {
            _spawner = spawner;
            _creator = creator;

            _creator.SpawnRequesterCreated += OnSpawnRequesterCreated;
        }

        private void OnSpawnRequesterCreated(ISpawnRequester<T> requester)
        {
            _requesters.Add(new SpawnRequestHandler<T>(_spawner, requester));
        }
    }
}
