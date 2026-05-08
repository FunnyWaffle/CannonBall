using Assets.Scripts.Shop;

namespace Assets.Scripts.Systems
{
    public class PlaceObjectSystem
    {
        //private readonly Spawner _spawner;

        //public PlaceObjectSystem(Spawner spawner)
        //{
        //    _spawner = spawner;
        //}

        public void Place(ItemTypes itemType)
        {
            var position = CameraSystem.MainCamera.GetFacedPosition();
            //var cannon = _spawner.Spawn<CannonController>(itemType, position, Quaternion.identity);
        }
    }
}
