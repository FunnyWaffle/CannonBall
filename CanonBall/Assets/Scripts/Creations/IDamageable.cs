namespace Assets.Scripts.Creations
{
    public interface IDamageable : IComponent
    {
        public void TakeDamage(float value);
    }
}
