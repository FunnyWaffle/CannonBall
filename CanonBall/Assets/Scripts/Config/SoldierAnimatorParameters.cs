using UnityEngine;

namespace Assets.Scripts.Config
{
    public static class SoldierAnimatorParameters
    {
        public readonly static int ForwardSpeed = Animator.StringToHash(nameof(ForwardSpeed));
        public readonly static int SideSpeed = Animator.StringToHash(nameof(SideSpeed));
        public readonly static int Jumped = Animator.StringToHash(nameof(Jumped));
    }
}
