using System;
using Xunit;

namespace TL.FeatureToggles.Tests
{
    [CollectionDefinition(nameof(FeatureToggleCollection))]
    public class FeatureToggleCollection : ICollectionFixture<FeatureToggleFixture>
    {
    }

    public class FeatureToggleFixture : IDisposable
    {
        public IToggleRouter ToggleRouter { get; }        

        public FeatureToggleFixture()
        {
            ToggleRouter = new ToggleRouter();
        }

        public void Dispose()
        {
        }
    }
}
