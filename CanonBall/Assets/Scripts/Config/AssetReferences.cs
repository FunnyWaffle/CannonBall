using Assets.Scripts.Shop;
using Assets.Scripts.Wrappers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Config
{
    [CreateAssetMenu(fileName = nameof(AssetReferences), menuName = "Config/" + nameof(AssetReferences))]
    public class AssetReferences : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<ItemType, AssetReference> _references;

        public AssetReference Get(ItemType itemType) => _references[itemType];
    }
}
