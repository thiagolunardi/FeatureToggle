namespace TL.FeatureToggles.Examples.InversionOfDecision;

public class InvoiceEmailer(Invoice invoice, InvoiceEmailerConfig config)
{
    public Email GenerateInvoiceEmail()
    {
        var baseEmail = BuildEmailForInvoice(invoice);

        return config.AddOrderCancellationContentToEmail
            ? AddOrderCancellationInEmail(baseEmail)
            : baseEmail;
    }

    // ... other invoice emailer methods ...

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