using System.Collections.Generic;

namespace TL.FeatureToggles.LayerOfIndirection
{
    public class InvoiceEmailer
    {
        private readonly FeatureDecisions _featureDecisions;

        public Invoice Invoice { get; protected set; }

        public InvoiceEmailer(IToggleRouter features)
        {
            _featureDecisions = new FeatureDecisions(features);
            Invoice = new Invoice();
        }

        public Email GenerateInvoiceEmail()
        {
            var baseEmail = BuildEmailForInvoice(this.Invoice);

            if (_featureDecisions.IncludeOrderCancellationInEmail())
                return AddOrderCancellationInEmail(baseEmail);
            else
                return baseEmail;
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

    public class Invoice { }
    public class OrderCancellation { }
    public class Email
    {
        public readonly List<object> Content = new List<object>();
    }
}
