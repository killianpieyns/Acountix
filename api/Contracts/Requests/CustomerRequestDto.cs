namespace api.Contracts.Requests;
public class CustomerRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public AddressRequestDto Address { get; set; } = new();
    public int Type { get; set; }
    public int CompanyId { get; set; }
}
