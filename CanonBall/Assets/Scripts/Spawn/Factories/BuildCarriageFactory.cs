using Assets.Scripts.Build.Carriage;
using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn.Factories
{
    public class BuildCarriageFactory : IUniversalFactory
    {
        private readonly DiContainer _diContainer;
        private readonly Updater _updater;

        public BuildCarriageFactory(DiContainer diContainer, Updater updater)
        {
            _diContainer = diContainer;
            _updater = updater;
        }

        public ItemType CreationType => ItemType.BuildCarriage;

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<BuildCarriage>();

            _diContainer.Inject(view);

            var components = new EntityComponents();
            components.Add(view);

            _updater.SetUpdatable(view);

            return components;
        }
    }
}
