namespace TL.FeatureToggles.Examples.LayerOfIndirection;

public class InvoiceEmailer(IToggleRouter features)
{
    private readonly FeatureDecisions _featureDecisions = new(features);

    public Invoice Invoice { get; } = new();

    public Email GenerateInvoiceEmail()
    {
        var baseEmail = BuildEmailForInvoice(Invoice);

        return _featureDecisions.IncludeOrderCancellationInEmail()
            ? AddOrderCancellationInEmail(baseEmail)
            : baseEmail;
    }

    private static Email BuildEmailForInvoice(Invoice invoice)
    {
        var baseEmail = new Email();
        baseEmail.Content.Add(invoice);

        return baseEmail;
    }

    private static Email AddOrderCancellationInEmail(Email email)
    {
        var orderCancellation = new OrderCancellation();
        email.Content.Add(orderCancellation);

        return email;
    }
}

public class Invoice
{
}

public class OrderCancellation
{
}

public class Email
{
    public readonly List<object> Content = [];
}