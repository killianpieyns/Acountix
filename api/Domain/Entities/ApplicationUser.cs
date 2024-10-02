using Microsoft.AspNetCore.Identity;

namespace api.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public List<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}