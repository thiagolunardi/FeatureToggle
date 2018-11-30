namespace TL.FeatureToggles
{
    public interface IToggleRouter
    {
        bool IsEnabled(string featureName);
        void SetFeature(string featureName, bool isEnabled);
    }
}