using api.Domain.Common;

namespace api.Domain.Entities;
public class Quotation : AuditBase
{
    public int Id { get; private set; }
    public string QuotationNumber { get; private set; }
    public DateTime Date { get; private set; }
    public decimal TotalAmount { get; private set; }
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public int CompanyId { get; private set; }
    public Company Company { get; private set; }
    public List<QuotationProduct> QuotationProducts { get; private set; } = new();
    public bool IsAccepted { get; private set; }
    public decimal Discount { get; private set; }
    public decimal DownPayment { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Quotation() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static Quotation Create(string quotationNumber, DateTime date, int customerId, int companyId, List<Product> products, decimal discount, decimal downPayment)
    {
        var quotation = new Quotation
        {
            QuotationNumber = quotationNumber,
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
            quotation.QuotationProducts.Add(new QuotationProduct { Product = product });
        }

        quotation.CalculateTotalAmount();

        return quotation;
    }

    public void Update(string quotationNumber, DateTime date, List<Product> products, decimal discount = 0, decimal downPayment = 0)
    {
        QuotationNumber = quotationNumber;
        Date = date;
        Discount = discount;
        DownPayment = downPayment;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";

        QuotationProducts.Clear();
        foreach (var product in products)
        {
            QuotationProducts.Add(QuotationProduct.Create(this, product));
        }

        CalculateTotalAmount();
    }

    private void CalculateTotalAmount()
    {
        var total = QuotationProducts.Sum(qp => qp.Product.Price * (1 + qp.Product.VATPercentage / 100));
        TotalAmount = total - Discount - DownPayment;
    }

    public void Accept()
    {
        IsAccepted = true;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }

    public void Refuse()
    {
        IsAccepted = false;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }

    public Invoice ConvertToInvoice()
    {
        return Invoice.Create($"INV-{QuotationNumber}", DateTime.UtcNow, CustomerId, CompanyId, QuotationProducts.Select(qp => qp.Product).ToList(), Discount, DownPayment);
    }

    public void NotifyDocumentOpened()
    {
        // Send email and notification logic here
    }
}
