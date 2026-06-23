using Assets.Scripts.Build;
using Assets.Scripts.Build.Carriage;
using Assets.Scripts.Creations.Placement;

namespace Assets.Scripts.Creations.Builder.Components
{
    public class Build : IComponent
    {
        private readonly IMover _mover;
        private readonly IHasPosition _hasPosition;

        private EntityComponents _construction;
        private IBuildCarriage _carriage;

        private bool _started = false;
        private bool _tookPart = false;

        public Build(IMover mover, IHasPosition hasPosition)
        {
            _mover = mover;
            _mover.Arrived += OnMoverArrival;

            _hasPosition = hasPosition;
        }

        public void Start(EntityComponents construction, IBuildCarriage buildCarriage)
        {
            _construction = construction;
            _carriage = buildCarriage;

            _started = true;
            _mover.SetDestination(buildCarriage.TrunkExit);
        }

        private void OnMoverArrival()
        {
            if (!_started)
                return;

            if (_tookPart)
            {
                var construction = _construction.Get<Construction>();
                construction.BuildNextPart();
                _tookPart = false;

                _mover.SetDestination(_carriage.TrunkExit);
            }
            else
            {
                _tookPart = true;

                var hasPosition = _construction.Get<IHasPosition>();
                _mover.SetDestination(hasPosition.Position);
            }
        }
    }
}
