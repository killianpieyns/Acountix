using api.Domain.Entities;
using api.Domain.ValueObjects;
using api.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Companies.Any())
        {
            var companies = new[]
            {
                Company.Create("Company A", Address.Create("123 Street", "CityA", "StateA", "12345", "CountryA"), "1234567890", "companya@example.com", "http://companya.com"),
                Company.Create("Company B", Address.Create("456 Avenue", "CityB", "StateB", "67890", "CountryB"), "0987654321", "companyb@example.com", "http://companyb.com")
            };

            context.Companies.AddRange(companies);
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var products = new[]
            {
                Product.Create("Product 1", "Description for product 1", 100, 0.21m, context.Companies.First(c => c.Name == "Company A").Id),
                Product.Create("Product 2", "Description for product 2", 200, 0.21m, context.Companies.First(c => c.Name == "Company B").Id)
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            var customers = new[]
            {
                Customer.Create("John Doe", "john.doe@example.com", "1234567890", Address.Create("123 Main St", "CityA", "StateA", "12345", "CountryA"), CustomerType.Private, context.Companies.First(c => c.Name == "Company A").Id),
                Customer.Create("Jane Smith", "jane.smith@example.com", "0987654321", Address.Create("456 Side St", "CityB", "StateB", "67890", "CountryB"), CustomerType.Business, context.Companies.First(c => c.Name == "Company B").Id),
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        if (!context.Invoices.Any())
        {
            var invoices = new[]
            {
                Invoice.Create("INV-001", DateTime.UtcNow, context.Customers.First(c => c.Name == "John Doe").Id, context.Companies.First(c => c.Name == "Company A").Id, context.Products.ToList(), 0m, 0m),
                Invoice.Create("INV-002", DateTime.UtcNow, context.Customers.First(c => c.Name == "Jane Smith").Id, context.Companies.First(c => c.Name == "Company B").Id, context.Products.ToList(), 10m, 20m)
            };

            foreach (var invoice in invoices)
            {
                invoice.GeneratePaymentLink("http://localhost:5000");
            }

            context.Invoices.AddRange(invoices);
            context.SaveChanges();
        }

        if (!context.Quotations.Any())
        {
            var quotations = new[]
            {
                Quotation.Create("QUO-001", DateTime.UtcNow, context.Customers.First(c => c.Name == "John Doe").Id, context.Companies.First(c => c.Name == "Company A").Id, context.Products.ToList(), 0m, 0m),
                Quotation.Create("QUO-002", DateTime.UtcNow, context.Customers.First(c => c.Name == "Jane Smith").Id, context.Companies.First(c => c.Name == "Company B").Id, context.Products.ToList(), 10m, 20m)
            };

            context.Quotations.AddRange(quotations);
            context.SaveChanges();
        }
    }
}
