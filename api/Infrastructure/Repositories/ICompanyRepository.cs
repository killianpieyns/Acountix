using api.Domain.Entities;

namespace api.Infrastructure.Repositories;

public interface ICompanyRepository
{
    Task AddAsync(Company company);
    Task AddCompanyUserAsync(CompanyUser companyUser);
    Task<Company> GetByIdAsync(int id);
    Task<IEnumerable<Company>> GetByUserIdAsync(string userId);
    Task UpdateAsync(Company company);
    Task DeleteAsync(int id);
}