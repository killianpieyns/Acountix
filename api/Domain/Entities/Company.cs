using api.Domain.Common;
using api.Domain.ValueObjects;

namespace api.Domain.Entities;

public class Company : AuditBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string Website { get; private set; }
    public List<CompanyUser> CompanyUsers { get; private set; } = new();
    public List<Customer> Customers { get; private set; } = new();
    public List<Invoice> Invoices { get; private set; } = new();
    public List<Quotation> Quotations { get; private set; } = new();
    public List<Product> Products { get; private set; } = new();

    private Company() { }

    public static Company Create(string name, Address address, string phone, string email, string website)
    {
        return new Company
        {
            Name = name,
            Address = address,
            Phone = phone,
            Email = email,
            Website = website,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedBy = "System",
            UpdatedBy = "System"
        };
    }

    public void Update(string name, Address address, string phone, string email, string website)
    {
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        Website = website;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }
}