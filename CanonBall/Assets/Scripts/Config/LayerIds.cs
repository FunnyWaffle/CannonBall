using UnityEngine;

namespace Assets.Scripts.Config
{
    public static class LayerIds
    {
        private static readonly string _vendor = "Vendor";
        private static readonly string _gun = "Gun";
        private static readonly string _player = "Player";
        private static readonly string _ground = "Ground";

        public static int Projectile { get; } = LayerMask.GetMask(nameof(Projectile));
        public static int BitMaskVendor { get; } = LayerMask.GetMask(_vendor);
        public static int IndexVendor { get; } = LayerMask.NameToLayer(_vendor);
        public static int BitMaskGun { get; } = LayerMask.GetMask(_gun);
        public static int IndexGun { get; } = LayerMask.NameToLayer(_gun);
        public static int BitMaskPlayer { get; } = LayerMask.GetMask(_player);
        public static int IndexPlayer { get; } = LayerMask.NameToLayer(_player);
        public static int BitMaskGround { get; } = LayerMask.GetMask(_ground);
        public static int IndexGround { get; } = LayerMask.NameToLayer(_ground);
    }
}
