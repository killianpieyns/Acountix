using api.Domain.Entities;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Repositories;

using api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task AddCompanyUserAsync(CompanyUser companyUser)
    {
        await _context.CompanyUsers.AddAsync(companyUser);
        await _context.SaveChangesAsync();
    }

    public async Task<Company> GetByIdAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.CompanyUsers)
            .Include(c => c.Customers)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Company>> GetByUserIdAsync(string userId)
    {
        return await _context.CompanyUsers
            .Where(cu => cu.UserId == userId)
            .Select(cu => cu.Company)
            .Include(c => c.CompanyUsers)
            .Include(c => c.Customers)
            .Include(c => c.Products)
            .ToListAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var company = await GetByIdAsync(id);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }
}
