namespace TL.FeatureToggles.TogglePoint
{
    public class SimulationEngine
    {
        private readonly SplinesReticulator _splinesReticulator;
        private readonly IToggleRouter _toggleRouter;

        public SimulationEngine(IToggleRouter toggleRouter)
        {
            _toggleRouter = toggleRouter;
            _splinesReticulator = new SplinesReticulator(_toggleRouter);
        }

        public Splines[] DoSomethingWhichInvolvesSplineReticulation()
            => _splinesReticulator.ReticulateSplines();
    }
}
