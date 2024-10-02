namespace api.Domain.Entities;

public class CompanyUser
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
