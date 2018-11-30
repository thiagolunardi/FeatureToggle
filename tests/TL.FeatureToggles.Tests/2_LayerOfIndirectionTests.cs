using System.Linq;
using FluentAssertions;
using TL.FeatureToggles.LayerOfIndirection;
using Xunit;

namespace TL.FeatureToggles.Tests
{
    [Collection(nameof(FeatureToggleCollection))]
    public class LayerOfIndirectionTests
    {
        private readonly IToggleRouter _toggleRouter;
        private readonly InvoiceEmailer _invoiceEmailer;

        public LayerOfIndirectionTests(FeatureToggleFixture fixture)
        {
            _toggleRouter = fixture.ToggleRouter;
            _invoiceEmailer = new InvoiceEmailer(_toggleRouter);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void It_works_correctly_with_any_algorithm(bool toggle)
        {
            // given
            _toggleRouter.SetFeature("next-gen-ecomm", toggle);

            // when
            var email = _invoiceEmailer.GenerateInvoiceEmail();

            // then
            VerifyInvoiceEmail(email);
        }

        private static void VerifyInvoiceEmail(Email email)
        {
            email.Content.Should().NotBeNull();
            email.Content.Any(c => c.GetType() == typeof(Invoice)).Should().BeTrue();
        }
    }
}
