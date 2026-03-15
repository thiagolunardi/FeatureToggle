namespace TL.FeatureToggles.Examples.InversionOfDecision;

public class FeatureAwareFactory
{
    private static FeatureAwareFactory? _factory;
    private readonly FeatureDecisions _featureDecisions;

    private FeatureAwareFactory(FeatureDecisions featureDecisions)
    {
        _featureDecisions = featureDecisions;
    }

    public static FeatureAwareFactory CreateFeatureAwareFactoryBasedOn(FeatureDecisions featureDecisions)
    {
        return _factory ??= new FeatureAwareFactory(featureDecisions);
    }

    public InvoiceEmailer CreateInvoiceEmailer(Invoice invoice)
    {
        return new InvoiceEmailer(invoice, new InvoiceEmailerConfig
        {
            AddOrderCancellationContentToEmail = _featureDecisions.IncludeOrderCancellationInEmail()
        });
    }

    // ... other factory methods ...
}