namespace TL.FeatureToggles.Examples.AvoidingConditionals;

public class InvoiceEmailer(Func<Email, Email> additionalContentEnhancer)
{
    public Email GenerateInvoiceEmail(Invoice invoice)
    {
        var baseEmail = BuildEmailForInvoice(invoice);
        return additionalContentEnhancer(baseEmail);
    }

    // ... other invoice emailer methods ...

    private static Email BuildEmailForInvoice(Invoice invoice)
    {
        var baseEmail = new Email();
        baseEmail.Content.Add(invoice);

        return baseEmail;
    }

    public static Email AddOrderCancellationInEmail(Email email)
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