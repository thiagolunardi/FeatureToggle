using System;

namespace TL.FeatureToggles.TogglePoint
{
    public class SplinesReticulator
    {
        private readonly IToggleRouter _features;

        public SplinesReticulator(IToggleRouter features)
        {
            _features = features;
        }

        public Splines[] ReticulateSplines()
        {
            if (_features.IsEnabled("use-new-SR-algorithm"))
                return EnhancedSplineReticulation();
            else
                return OldFashionedSplineReticulation();
        }

        private Splines[] OldFashionedSplineReticulation()
        {
            return new Splines[0];
        }

        private Splines[] EnhancedSplineReticulation()
        {
            return Array.Empty<Splines>();
        }
    }

    public class Splines { }
}
