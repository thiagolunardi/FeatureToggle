namespace TL.FeatureToggles.Examples.TogglePoint;

public class SplinesReticulator(IToggleRouter features)
{
    public Splines[] ReticulateSplines()
    {
        return features.IsEnabled("use-new-SR-algorithm")
            ? EnhancedSplineReticulation()
            : OldFashionedSplineReticulation();
    }

    private Splines[] OldFashionedSplineReticulation()
    {
        return Array.Empty<Splines>();
    }

    private Splines[] EnhancedSplineReticulation()
    {
        return [];
    }
}

public class Splines
{
}