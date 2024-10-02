using api.Domain.Common;

namespace api.Domain.Entities;
public class Product : AuditBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public decimal VATPercentage { get; private set; }
    public int CompanyId { get; private set; }
    public Company Company { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Product() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static Product Create(string name, string description, decimal price, decimal vatPercentage, int companyId)
    {
        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            VATPercentage = vatPercentage,
            CompanyId = companyId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedBy = "System",
            UpdatedBy = "System"
        };
    }

    public void Update(string name, string description, decimal price, decimal vatPercentage)
    {
        Name = name;
        Description = description;
        Price = price;
        VATPercentage = vatPercentage;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }
}
