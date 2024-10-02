namespace api.Domain.Entities;
public class InvoiceProduct
{
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public decimal Quantity { get; set; }
    public decimal Amount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public InvoiceProduct() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static InvoiceProduct Create(Invoice invoice, Product product)
    {
        return new InvoiceProduct
        {
            Invoice = invoice,
            InvoiceId = invoice.Id,
            Product = product,
            ProductId = product.Id
        };
    }
}