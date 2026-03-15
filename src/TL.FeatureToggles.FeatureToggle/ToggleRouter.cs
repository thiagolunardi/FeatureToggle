using System;
using System.Collections.Generic;

namespace TL.FeatureToggles;

public class ToggleRouter : IToggleRouter
{
    private readonly Dictionary<string, bool> _featureConfig = new();

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

    public bool IsEnabled(string featureName)
    {
        AssertFeatureName(featureName);

        return _featureConfig.GetValueOrDefault(featureName, false);
    }

    private static void AssertFeatureName(string featureName)
    {
        ArgumentException.ThrowIfNullOrEmpty(featureName);
    }
}