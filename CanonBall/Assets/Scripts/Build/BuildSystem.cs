using Assets.Scripts.Build.Carriage;
using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Build
{
    public class BuildSystem
    {
        private readonly UniversalSpawner _spawner;
        private readonly BuilderSpawnZone _builderSpawnZone;
        private readonly CoroutineRunner _coroutineRunner;

        private readonly float _builderRadius = .5f;

        public BuildSystem(UniversalSpawner spawner, BuilderSpawnZone builderSpawnZone, CoroutineRunner coroutineRunner)
        {
            _spawner = spawner;
            _builderSpawnZone = builderSpawnZone;
            _coroutineRunner = coroutineRunner;
        }

        public async Task<ConstructionOrder> OrderConstruction(
            Vector3 position,
            Quaternion rotation,
            float positionForCarriageOffset)
        {
            var order = new ConstructionOrder(position, rotation, positionForCarriageOffset, _builderSpawnZone, _spawner);

            var spawnPosition = _builderSpawnZone.GetPosition();

            var direction = Vector3.Normalize(position - spawnPosition);
            var rotationToTarget = Quaternion.LookRotation(direction, Vector3.up);

            var carriageComponents = await _spawner.SpawnAsync(ItemTypes.BuildCarriage, spawnPosition, rotationToTarget);
            order.SetCarriage(carriageComponents);

            var mover = carriageComponents.Get<IMover>();
            mover.SetDestination(position + -direction * positionForCarriageOffset);

            var carriage = carriageComponents.Get<IBuildCarriage>();
            var trunkOffset = carriage.TrunkOffset;

            while (true)
            {
                for (int i = 0; i < 5; i++)
                {
                    var builder = await _spawner.SpawnAsync(ItemTypes.Builder, spawnPosition, rotationToTarget);

                    order.SetBuilder(builder);

                    mover = builder.Get<IMover>();
                    mover.SetDestination(spawnPosition + direction * (trunkOffset + _builderRadius));
                }

                if (_builderSpawnZone.CanAccommodate())
                    break;
                else
                    await Task.Yield();
            }

            return order;
        }
    }
}
