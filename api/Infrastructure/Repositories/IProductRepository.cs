using System.Collections.Generic;
using api.Domain.Entities;

namespace api.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetByIdsAsync(List<int> ids);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        void Delete(Product product);
    }
}
