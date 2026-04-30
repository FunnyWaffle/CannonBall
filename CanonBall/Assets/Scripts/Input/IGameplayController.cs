namespace Assets.Scripts.Input
{
    public interface IGameplayController
    {
        public void HandleInput(InputData input);
        public void TransferCamera();
    }
}
