using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Builder;
using Assets.Scripts.Creations.Builder.Components;
using Assets.Scripts.Shop;
using Assets.Scripts.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Spawn.Factories
{
    public class BuilderFactory : IUniversalFactory
    {
        private readonly DiContainer _diContainer;
        private readonly Updater _updater;

        public BuilderFactory(DiContainer diContainer, Updater updater)
        {
            _diContainer = diContainer;
            _updater = updater;
        }

        public ItemTypes CreationType => ItemTypes.Builder;

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var gameObject = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = gameObject.GetComponent<BuilderView>();

            var mover = new BuilderMover(view.Agent, view);
            var build = _diContainer.Instantiate<Creations.Builder.Components.Build>(new object[] { mover, view });
            var components = new EntityComponents();

            components.Add(mover);
            components.Add(build);

            var update = new BuilderUpdate(mover);
            _updater.SetUpdatable(update);

            return components;
        }
    }
}
