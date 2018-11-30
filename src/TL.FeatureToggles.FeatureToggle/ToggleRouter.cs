using System;
using System.Collections.Generic;

namespace TL.FeatureToggles
{
    public class ToggleRouter : IToggleRouter
    {
        private Dictionary<string, bool> _featureConfig = new Dictionary<string, bool>();

        public void SetFeature(string featureName, bool isEnabled)
        {
            AssertFeatureName(featureName);

            if (_featureConfig.TryGetValue(featureName, out var value))
            {
                if (value.Equals(isEnabled)) return;
                _featureConfig.Remove(featureName);
            }
            _featureConfig.Add(featureName, isEnabled);
        }

        private static void AssertFeatureName(string featureName)
        {
            if (string.IsNullOrEmpty(featureName) || string.IsNullOrWhiteSpace(featureName))
                throw new ArgumentNullException(nameof(featureName));
        }

        public bool IsEnabled(string featureName)
        {
            AssertFeatureName(featureName);

            _featureConfig.TryGetValue(featureName, out var isEnabled);
            return isEnabled;
        }
    }
}
