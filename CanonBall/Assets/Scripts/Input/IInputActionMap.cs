namespace Assets.Scripts.Input
{
    public interface IInputActionMap
    {
        public InputType Type { get; }

        public void Enable();
        public void Disable();
    }
}
