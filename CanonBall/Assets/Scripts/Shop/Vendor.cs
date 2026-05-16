using Assets.Scripts.Interaction;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Shop
{
    public class Vendor : MonoBehaviour, IItemSeller
    {
        public ItemTypes[] ItemsForSale;
        public Collider Collider;

        [Inject]
        private void Initialize(InteractionObjectsRepositiory interactionObjectsRepositiory)
        {
            interactionObjectsRepositiory.AddInteactionable(Collider, InteractionableTypes.Vendor);
            interactionObjectsRepositiory.AddItemSeller(Collider, this);
        }

        public IEnumerable<ItemTypes> GetItems()
        {
            return ItemsForSale;
        }
    }
}
