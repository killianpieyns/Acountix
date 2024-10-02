namespace api.Contracts.Requests;

public class RefreshTokenRequestDto
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}