using FluentAssertions;
using Xunit;

namespace TL.FeatureToggles.Tests
{
    [Collection(nameof(FeatureToggleCollection))]
    public class ToggleRouterTests
    {
        private readonly IToggleRouter _toggleRouter;

        public ToggleRouterTests(FeatureToggleFixture fixture)
        {
            _toggleRouter = fixture.ToggleRouter;
        }

        [Theory]
        [InlineData(true), InlineData(false)]
        public void It_should_create_feature_toggle(bool toggle)
        {
            // given
            _toggleRouter.SetFeature("new-feature", toggle);

            // when
            var isEnabled = _toggleRouter.IsEnabled("new-feature");

            // then
            isEnabled.Should().Be(toggle);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void It_should_change_feature_toggle_value(bool initialValue, bool newValue)
        {
            // given
            _toggleRouter.SetFeature("new-feature", initialValue);
            var firstValue = _toggleRouter.IsEnabled("new-feature");

            // when
            _toggleRouter.SetFeature("new-feature", newValue);
            var secondValue = _toggleRouter.IsEnabled("new-feature");

            // then
            firstValue.Should().Be(initialValue);
            secondValue.Should().Be(newValue);
        }
    }
}
