using api.Domain.Common;
using api.Domain.ValueObjects;

namespace api.Domain.Entities;
public enum CustomerType
{
    Private,
    Business
}

public class Customer : AuditBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public Address Address { get; private set; }
    public CustomerType Type { get; private set; }
    public int CompanyId { get; private set; }
    public Company Company { get; private set; }

    public List<Quotation> Quotations { get; private set; } = new();
    public List<Invoice> Invoices { get; private set; } = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Customer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static Customer Create(string name, string email, string phone, Address address, CustomerType type, int companyId)
    {
        return new Customer
        {
            Name = name,
            Email = email,
            Phone = phone,
            Address = address,
            Type = type,
            CompanyId = companyId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            CreatedBy = "System",
            UpdatedBy = "System"
        };
    }

    public void Update(string name, string email, string phone, Address address, CustomerType type)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        Type = type;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "System";
    }

    public void AcceptQuotation(Quotation quotation)
    {
        var existingQuotation = Quotations.FirstOrDefault(q => q.Id == quotation.Id);
        if (existingQuotation != null)
        {
            existingQuotation.Accept();
        }
    }

    public void RefuseQuotation(Quotation quotation)
    {
        var existingQuotation = Quotations.FirstOrDefault(q => q.Id == quotation.Id);
        if (existingQuotation != null)
        {
            existingQuotation.Refuse();
        }
    }

    public void NotifyDocumentOpened(string documentType, int documentId)
    {
        // Send email and notification logic here
    }
}
