using System.Collections.Generic;
using api.Domain.Entities;

namespace api.Infrastructure.Repositories;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAll();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    void Delete(Customer customer);
}
