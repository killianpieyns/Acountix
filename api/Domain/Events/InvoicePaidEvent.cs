using System;

namespace Api.Domain.Events
{
    public class InvoicePaidEvent
    {
        public int InvoiceId { get; }
        public DateTime PaidDate { get; }

        public InvoicePaidEvent(int invoiceId)
        {
            InvoiceId = invoiceId;
            PaidDate = DateTime.UtcNow;
        }
    }
}
