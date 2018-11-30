using System.Linq;
using FluentAssertions;
using TL.FeatureToggles.InversionOfDecision;
using Xunit;

namespace TL.FeatureToggles.Tests
{
    [Collection(nameof(FeatureToggleCollection))]
    public class AvoidingConditionalsTests
    {
        private readonly IToggleRouter _toggleRouter;
        private readonly FeatureDecisions _featureDecisions;

        public AvoidingConditionalsTests(FeatureToggleFixture fixture)
        {
            _toggleRouter = fixture.ToggleRouter;
            _featureDecisions = new FeatureDecisions(_toggleRouter);            
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void It_works_correctly_with_any_algorithm(bool toggle, bool anyOrderCancellationContent)
        {
            // given            
            _toggleRouter.SetFeature("next-gen-ecomm", toggle);
            var factory = FeatureAwareFactory.CreateFeatureAwareFactoryBasedOn(_featureDecisions);            
            var emailer = factory.CreateInvoiceEmailer(new Invoice());

            // when
            var email = emailer.GenerateInvoiceEmail();

            // then
            VerifyEmailContent(email, anyOrderCancellationContent);
        }

        private static void VerifyEmailContent(Email email, bool anyOrderCancellationContent)
        {
            email.Content.Should().NotBeNull();
            email.Content.Any(c => c.GetType() == typeof(Invoice)).Should().BeTrue();
            email.Content.Any(c => c.GetType() == typeof(OrderCancellation)).Should().Be(anyOrderCancellationContent);
        }
    }
}
