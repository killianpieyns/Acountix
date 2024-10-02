using System.Collections.Generic;
using api.Domain.Entities;

namespace api.Infrastructure.Repositories;

public interface IQuotationRepository
{
    Task<Quotation?> GetByIdAsync(int id);
    Task<IEnumerable<Quotation>> GetAllAsync();
    Task AddAsync(Quotation quotation);
    Task UpdateAsync(Quotation quotation);
    Task DeleteAsync(Quotation quotation);
}
