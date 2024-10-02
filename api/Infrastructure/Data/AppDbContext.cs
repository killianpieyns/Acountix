using Microsoft.EntityFrameworkCore;
using api.Domain.Entities;
using api.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Quotation> Quotations { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Audit Properties
        // Configure DateTime properties to use UTC
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }
        }

        // Value converter for CustomerType enum
        var customerTypeConverter = new ValueConverter<CustomerType, string>(
            v => v.ToString(),
            v => (CustomerType)Enum.Parse(typeof(CustomerType), v)
        );

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => new { e.Token, e.UserId }); // Composite key

            entity.HasOne(e => e.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasMany(u => u.RefreshTokens)
                  .WithOne(r => r.User)
                  .HasForeignKey(r => r.UserId);
        });

        modelBuilder.Entity<CompanyUser>(entity =>
        {
            entity.ToTable("CompanyUsers");
            entity.HasKey(cu => new { cu.UserId, cu.CompanyId });

            entity.HasOne(cu => cu.Company)
                  .WithMany(c => c.CompanyUsers)
                  .HasForeignKey(cu => cu.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cu => cu.User)
                  .WithMany(u => u.CompanyUsers)
                  .HasForeignKey(cu => cu.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200).IsRequired();
                address.Property(a => a.City).HasMaxLength(100).IsRequired();
                address.Property(a => a.State).HasMaxLength(100).IsRequired();
                address.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
                address.Property(a => a.Country).HasMaxLength(100).IsRequired();
            });
            entity.Property(c => c.Phone).HasMaxLength(15);
            entity.Property(c => c.Email).HasMaxLength(100);
            entity.Property(c => c.Website).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Phone).HasMaxLength(15).IsRequired();
            entity.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200).IsRequired();
                address.Property(a => a.City).HasMaxLength(100).IsRequired();
                address.Property(a => a.State).HasMaxLength(100).IsRequired();
                address.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
                address.Property(a => a.Country).HasMaxLength(100).IsRequired();
            });
            entity.Property(c => c.Type).IsRequired();

            entity.HasOne(c => c.Company)
                  .WithMany(company => company.Customers)
                  .HasForeignKey(c => c.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.Property(p => p.Price).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(p => p.VATPercentage).HasColumnType("decimal(18, 2)").IsRequired();

            entity.HasOne(p => p.Company)
                  .WithMany(company => company.Products)
                  .HasForeignKey(p => p.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.ToTable("Quotations");
            entity.HasKey(q => q.Id);
            entity.Property(q => q.QuotationNumber).IsRequired().HasMaxLength(100);
            entity.Property(q => q.Date).IsRequired().HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(q => q.TotalAmount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(q => q.IsAccepted).IsRequired();
            entity.Property(q => q.Discount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(q => q.DownPayment).HasColumnType("decimal(18, 2)").IsRequired();

            entity.HasOne(q => q.Customer)
                  .WithMany(c => c.Quotations)
                  .HasForeignKey(q => q.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(q => q.Company)
                  .WithMany(company => company.Quotations)
                  .HasForeignKey(q => q.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuotationProduct>(entity =>
        {
            entity.HasKey(qp => new { qp.QuotationId, qp.ProductId });

            entity.HasOne(qp => qp.Quotation)
                  .WithMany(q => q.QuotationProducts)
                  .HasForeignKey(qp => qp.QuotationId);

            entity.HasOne(qp => qp.Product)
                  .WithMany()
                  .HasForeignKey(qp => qp.ProductId);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(50);
            entity.Property(i => i.Date).IsRequired().HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(i => i.TotalAmount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(i => i.IsPaid).IsRequired();
            entity.Property(i => i.PaymentLink).HasAnnotation("MaxLength", 200);
            entity.Property(i => i.Discount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(i => i.DownPayment).HasColumnType("decimal(18, 2)").IsRequired();

            entity.HasOne(i => i.Customer)
                  .WithMany(c => c.Invoices)
                  .HasForeignKey(i => i.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Company)
                  .WithMany(company => company.Invoices)
                  .HasForeignKey(i => i.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceProduct>(entity =>
        {
            entity.HasKey(ip => new { ip.InvoiceId, ip.ProductId });

            entity.HasOne(ip => ip.Invoice)
                  .WithMany(i => i.InvoiceProducts)
                  .HasForeignKey(ip => ip.InvoiceId);

            entity.HasOne(ip => ip.Product)
                  .WithMany()
                  .HasForeignKey(ip => ip.ProductId);
        });
    }
}
