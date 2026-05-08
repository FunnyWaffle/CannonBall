using Assets.Scripts.Spawn.Factories;

namespace Assets.Scripts.Spawn
{
    public class SpawnRequestHandler<T>
        where T : class, ISpawnable, IPoolableObject
    {
        private readonly Spawner<T> _spawner;
        private readonly ISpawnRequesterCreator<T> _creator;

        public SpawnRequestHandler(Spawner<T> spawner, ISpawnRequesterCreator<T> creator)
        {
            _spawner = spawner;
            _creator = creator;

            _creator.SpawnRequesterCreated += OnSpawnRequesterCreated;
        }

        private void OnSpawnRequesterCreated(ISpawnRequester<T> requester)
        {
            requester.SpawnRequested += OnSpawnRequestReceived;
        }

        private async void OnSpawnRequestReceived(object requester, SpawnArguments arguments)
        {
            var spawnedObject = await _spawner.Spawn(arguments.ItemType, arguments.Position, arguments.Rotation, arguments.Parent);
            var typedRequester = (ISpawnRequester<T>)requester;
            typedRequester.SetSpawnedObject(spawnedObject);
        }
    }
}
