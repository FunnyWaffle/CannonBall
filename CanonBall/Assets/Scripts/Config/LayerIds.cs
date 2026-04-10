using UnityEngine;

namespace Assets.Scripts.Config
{
    public static class LayerIds
    {
        public static int Projectile { get; } = LayerMask.GetMask("Projectile");
    }
}
