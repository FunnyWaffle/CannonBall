using Assets.Scripts.Shop;
using Assets.Scripts.Wrappers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Config
{
    [CreateAssetMenu(fileName = nameof(AssetReferences), menuName = "Config/" + nameof(AssetReferences))]
    public class AssetReferences : ScriptableObject
    {
        public SerializableDictionary<ItemTypes, AssetReference> References;

        public AssetReference Get(ItemTypes itemType)
        {
            return References[itemType];
        }
    }
}
