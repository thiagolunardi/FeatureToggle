namespace TL.FeatureToggles.Examples;

public class FeatureDecisions(IToggleRouter features)
{
    public bool IncludeOrderCancellationInEmail()
    {
        return features.IsEnabled("next-gen-ecomm");
    }
}