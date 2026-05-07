namespace Assets.Scripts.Input
{
    public interface IController
    {
        public void HandleInput(InputData input);
        public void TransferCamera();
    }
}
