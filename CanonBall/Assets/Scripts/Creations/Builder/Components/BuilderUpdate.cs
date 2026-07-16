namespace Assets.Scripts.Creations.Builder.Components
{
    public class BuilderUpdate : IUpdatable
    {
        private readonly BuilderMover _builderMover;

        public BuilderUpdate(BuilderMover builderMover)
        {
            _builderMover = builderMover;
        }

        public void Update()
        {
            _builderMover.Update();
        }
    }
}
