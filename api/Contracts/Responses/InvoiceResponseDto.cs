
namespace api.Contracts.Responses;
public class InvoiceResponseDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public decimal Discount { get; set; }
    public decimal DownPayment { get; set; }
    public bool IsPaid { get; set; }
    public CustomerResponseDto Customer { get; set; } = new();
    public List<ProductResponseDto> Products { get; set; } = new();
    public int CompanyId { get; internal set; }
}
