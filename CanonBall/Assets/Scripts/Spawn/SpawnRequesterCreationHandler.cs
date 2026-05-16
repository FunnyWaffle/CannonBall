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

    public class SpawnRequestHandler<T>
        where T : ISpawnable, IPoolableObject
    {
        private readonly Spawner<T> _spawner;
        private readonly ISpawnRequester<T> _requester;

        public SpawnRequestHandler(Spawner<T> spawner, ISpawnRequester<T> requester)
        {
            _spawner = spawner;
            _requester = requester;

            _requester.SpawnRequested += OnSpawnRequestReceived;
        }

        private async void OnSpawnRequestReceived(object requester, SpawnArguments arguments)
        {
            var spawnedObject = await _spawner.Spawn(arguments.ItemType, arguments.Position, arguments.Rotation, arguments.Parent);
            var typedRequester = (ISpawnRequester<T>)requester;
            typedRequester.SetSpawnedObject(spawnedObject);
        }
    }
}
