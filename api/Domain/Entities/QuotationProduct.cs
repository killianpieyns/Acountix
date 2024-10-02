namespace api.Domain.Entities;
public class QuotationProduct
{
    public int QuotationId { get; set; }
    public Quotation Quotation { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public decimal Quantity { get; set; }
    public decimal Amount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public QuotationProduct() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static QuotationProduct Create(Quotation quotation, Product product)
    {
        return new QuotationProduct
        {
            Quotation = quotation,
            QuotationId = quotation.Id,
            Product = product,
            ProductId = product.Id
        };
    }
}
