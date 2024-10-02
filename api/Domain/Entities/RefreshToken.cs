namespace api.Domain.Entities;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !IsExpired;
    public string CreatedByIp { get; set; }
    public string CreatedByUserAgent { get; set; }
    public string JwtId { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
