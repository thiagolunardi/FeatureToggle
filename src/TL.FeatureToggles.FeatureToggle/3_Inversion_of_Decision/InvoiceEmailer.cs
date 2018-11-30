using System.Collections.Generic;

namespace TL.FeatureToggles.InversionOfDecision
{
    public class InvoiceEmailer
    {
        private readonly InvoiceEmailerConfig _config;
        private readonly Invoice _invoice;

        public InvoiceEmailer(Invoice invoice, InvoiceEmailerConfig config)
        {
            _config = config;
            _invoice = invoice;
        }

        public Email GenerateInvoiceEmail()
        {
            var baseEmail = BuildEmailForInvoice(_invoice);

            if (_config.AddOrderCancellationContentToEmail)
                return AddOrderCancellationInEmail(baseEmail);
            else
                return baseEmail;
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

    public class Invoice { }
    public class OrderCancellation { }
    public class Email
    {
        public readonly List<object> Content = new List<object>();
    }
}
