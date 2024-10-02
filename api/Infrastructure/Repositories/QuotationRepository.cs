using System.Collections.Generic;
using System.Linq;
using api.Domain.Entities;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Repositories;

public class QuotationRepository : IQuotationRepository
{
    private readonly AppDbContext _context;

    public QuotationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Quotation?> GetByIdAsync(int id)
    {
        return await _context.Quotations
            .Include(q => q.Customer)
            .Include(q => q.QuotationProducts)
            .ThenInclude(qp => qp.Product)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<IEnumerable<Quotation>> GetAllAsync()
    {
        return await _context.Quotations
            .Include(q => q.Customer)
            .Include(q => q.QuotationProducts)
            .ThenInclude(qp => qp.Product)
            .ToListAsync();
    }

    public async Task AddAsync(Quotation quotation)
    {
        await _context.Quotations.AddAsync(quotation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Quotation quotation)
    {
        _context.Quotations.Update(quotation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Quotation quotation)
    {
        _context.Quotations.Remove(quotation);
        await _context.SaveChangesAsync();
    }
}
