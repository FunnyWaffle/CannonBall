using UnityEngine;


namespace Assets.Scripts.Config
{
    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "Config/" + nameof(PlayerConfig))]
    public class PlayerConfig : ScriptableObject
    {
        public float Sensitivity = 10f;
        public float Speed = 5f;
    }
}