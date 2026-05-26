namespace Assets.Scripts.Spawn
{
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
