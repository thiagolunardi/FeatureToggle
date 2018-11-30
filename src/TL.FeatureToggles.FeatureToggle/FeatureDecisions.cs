namespace TL.FeatureToggles
{
    public class FeatureDecisions
    {
        private readonly IToggleRouter _features;

        public FeatureDecisions(IToggleRouter features)
        {
            _features = features;
        }

        public bool IncludeOrderCancellationInEmail()
            => _features.IsEnabled("next-gen-ecomm");
    }
}
