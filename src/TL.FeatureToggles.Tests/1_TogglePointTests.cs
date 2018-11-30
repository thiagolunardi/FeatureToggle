using FluentAssertions;
using TL.FeatureToggles.TogglePoint;
using Xunit;

namespace TL.FeatureToggles.Tests
{
    [Collection(nameof(FeatureToggleCollection))]
    public class TogglePointTests
    {
        private readonly IToggleRouter _toggleRouter;
        private readonly SimulationEngine _simulationEngine;

        public TogglePointTests(FeatureToggleFixture fixture)
        {
            _toggleRouter = fixture.ToggleRouter;
            _simulationEngine = new SimulationEngine(_toggleRouter);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void It_works_correctly_with_any_algorithm(bool toggle)
        {
            // given
            _toggleRouter.SetFeature("use-new-SR-algorithm", toggle);

            // when
            var result = _simulationEngine.DoSomethingWhichInvolvesSplineReticulation();

            // then
            VerifySplineReticulation(result);
        }

        private static void VerifySplineReticulation(Splines[] result)
        {
            result.Should().HaveCount(0);
        }
    }
}
