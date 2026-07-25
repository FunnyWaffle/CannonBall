using UnityEngine;


namespace Assets.Scripts.Config
{
    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "Config/" + nameof(PlayerConfig))]
    public class PlayerConfig : ScriptableObject
    {
        public float Sensitivity = 10f;
        public float Speed = 5f;
        public float JumpPower = 5f;
        public float MaxVelocity = 5f;
        public float MovementAcceleration = 5f;
        public float MovementDeceleration = 5f;
        public float MovementAirAcceleration = 5f;
        public float MovementAirDeceleration = 5f;
        public float MaxInteractionDistance = 1f;
    }
}