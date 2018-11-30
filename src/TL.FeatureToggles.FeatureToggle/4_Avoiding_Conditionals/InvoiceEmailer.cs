using System;
using System.Collections.Generic;

namespace TL.FeatureToggles.AvoidingConditionals
{
    public class InvoiceEmailer
    {
        private readonly Func<Email, Email> _additionalContentEnhancer;
        public InvoiceEmailer(Func<Email, Email> additionalContentEnhancer)
        {
            _additionalContentEnhancer = additionalContentEnhancer;
        }

        public Email GenerateInvoiceEmail(Invoice invoice)
        {
            var baseEmail = BuildEmailForInvoice(invoice);
            return _additionalContentEnhancer(baseEmail);
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

    public class Invoice { }
    public class OrderCancellation { }
    public class Email
    {
        public readonly List<object> Content = new List<object>();
    }
}
