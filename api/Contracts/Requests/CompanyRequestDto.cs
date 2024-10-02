namespace api.Contracts.Requests;

public class CompanyRequestDto
{
    public string Name { get; set; } = string.Empty;
    public AddressRequestDto Address { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
}