using UnityEngine;

namespace Assets.Scripts.Config
{
    public static class TagIds
    {
        public static TagHandle Projectile { get; } = TagHandle.GetExistingTag("Projectile");
    }
}