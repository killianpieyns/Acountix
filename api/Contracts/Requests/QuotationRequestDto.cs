namespace api.Contracts.Requests;
public class QuotationRequestDto
{
    public string QuotationNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public decimal Discount { get; set; }
    public decimal DownPayment { get; set; }
    public int CompanyId { get; internal set; }
}
