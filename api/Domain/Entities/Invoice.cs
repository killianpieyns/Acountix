using api.Domain.Common;

namespace api.Domain.Entities;
public class Invoice : AuditBase
{
    public int Id { get; private set; }
    public string InvoiceNumber { get; private set; }
    public DateTime Date { get; private set; }
    public decimal TotalAmount { get; private set; }
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public int CompanyId { get; private set; }
    public Company Company { get; private set; }
    public List<InvoiceProduct> InvoiceProducts { get; private set; } = [];
    public bool IsPaid { get; private set; }
    public string? PaymentLink { get; private set; }
    public decimal Discount { get; private set; }
    public decimal DownPayment { get; private set; }
    public decimal Tax { get; private set; }
    public decimal TaxRate { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Invoice() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static Invoice Create(
        string invoiceNumber,
        DateTime date,
        int customerId,
        int companyId,
        List<Product> products,
        decimal discount,
        decimal downPayment)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = invoiceNumber,
            Date = date,
            CustomerId = customerId,
            CompanyId = companyId,
            Discount = discount,
            DownPayment = downPayment,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedBy = "System",
            UpdatedBy = "System"
        };

        foreach (var product in products)
        {
            invoice.InvoiceProducts.Add(new InvoiceProduct { Product = product });
        }

        invoice.CalculateTotalAmount();

        return invoice;
    }

    public void Update(string invoiceNumber, DateTime date, List<Product> products, decimal discount = 0, decimal downPayment = 0)
    {
        InvoiceNumber = invoiceNumber;
        Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        Discount = discount;
        DownPayment = downPayment;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";

        InvoiceProducts.Clear();
        foreach (var product in products)
        {
            InvoiceProducts.Add(InvoiceProduct.Create(this, product));
        }

        CalculateTotalAmount();
    }

    private void CalculateTotalAmount()
    {
        var total = InvoiceProducts.Sum(ip => ip.Product.Price * (1 + ip.Product.VATPercentage / 100));
        TotalAmount = total - Discount - DownPayment;
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }

    public void GeneratePaymentLink(string baseUrl)
    {
        PaymentLink = $"{baseUrl}/payment/{Id}";
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }

    public void NotifyDocumentOpened()
    {
        // Send email and notification logic here
    }
}
