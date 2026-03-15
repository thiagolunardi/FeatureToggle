namespace TL.FeatureToggles.Examples.TogglePoint;

public class SimulationEngine(IToggleRouter toggleRouter)
{
    private readonly SplinesReticulator _splinesReticulator = new(toggleRouter);

    public Splines[] DoSomethingWhichInvolvesSplineReticulation()
    {
        return _splinesReticulator.ReticulateSplines();
    }
}