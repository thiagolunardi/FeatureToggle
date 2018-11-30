using System;

namespace TL.FeatureToggles.AvoidingConditionals
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

        public InvoiceEmailer CreateInvoiceEmailer()
        {
            Func<Email, Email> identifyFn = x => x;

            return _featureDecisions.IncludeOrderCancellationInEmail()
                ? new InvoiceEmailer(InvoiceEmailer.AddOrderCancellationInEmail)
                : new InvoiceEmailer(identifyFn);
        }

        // ... other factory methods ...
    }
}
