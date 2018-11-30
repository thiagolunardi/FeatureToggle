namespace TL.FeatureToggles.InversionOfDecision
{
    public class FeatureAwareFactory
    {
        private readonly FeatureDecisions _featureDecisions;
        private static FeatureAwareFactory _factory;

        private FeatureAwareFactory(FeatureDecisions featureDecisions)
        {
            _featureDecisions = featureDecisions;
        }

        public static FeatureAwareFactory CreateFeatureAwareFactoryBasedOn(FeatureDecisions featureDecisions)
            => _factory ?? (_factory = new FeatureAwareFactory(featureDecisions));

        public InvoiceEmailer CreateInvoiceEmailer(Invoice invoice)
            => new InvoiceEmailer(invoice, new InvoiceEmailerConfig
            {
                AddOrderCancellationContentToEmail = _featureDecisions.IncludeOrderCancellationInEmail()
            });

        // ... other factory methods ...
    }
}
