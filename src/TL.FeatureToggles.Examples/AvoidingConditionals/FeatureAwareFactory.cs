namespace TL.FeatureToggles.Examples.AvoidingConditionals;

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

    public InvoiceEmailer CreateInvoiceEmailer()
    {
        Email IdentifyFn(Email x)
        {
            return x;
        }

        return _featureDecisions.IncludeOrderCancellationInEmail()
            ? new InvoiceEmailer(InvoiceEmailer.AddOrderCancellationInEmail)
            : new InvoiceEmailer(IdentifyFn);
    }

    // ... other factory methods ...
}