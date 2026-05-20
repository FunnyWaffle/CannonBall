namespace Assets.Scripts.Input
{
    public interface IInputSheme
    {
        public InputType Type { get; }

        public void Enable();
        public void Disable();
    }
}
