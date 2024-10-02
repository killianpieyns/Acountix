using api.Contracts.Requests;
using api.Contracts.Responses;
using api.Domain.Entities;
using api.Domain.ValueObjects;
using api.Infrastructure.Repositories;
using api.Presentation.Mappings;

namespace api.Application.Services;

public class CompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;

    public CompanyService(ICompanyRepository companyRepository, ICustomerRepository customerRepository, IProductRepository productRepository)
    {
        _companyRepository = companyRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<CompanyResponseDto> AddCompanyAsync(CompanyRequestDto companyRequest, string userId)
    {
        var company = Company.Create(
            companyRequest.Name,
            Address.Create(companyRequest.Address.Street, companyRequest.Address.City, companyRequest.Address.State, companyRequest.Address.PostalCode, companyRequest.Address.Country),
            companyRequest.Phone,
            companyRequest.Email,
            companyRequest.Website
        );

        var companyUser = new CompanyUser
        {
            UserId = userId,
            Company = company
        };

        await _companyRepository.AddAsync(company);
        await _companyRepository.AddCompanyUserAsync(companyUser);

        return company.ToResponseDto();
    }

    public async Task<CompanyResponseDto> GetCompanyByIdAsync(int id, string userId)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null || !company.CompanyUsers.Any(cu => cu.UserId == userId))
        {
            return null;
        }
        return company.ToResponseDto();
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync(string userId)
    {
        var companies = await _companyRepository.GetByUserIdAsync(userId);
        return companies.Select(c => c.ToResponseDto());
    }

    public async Task<CompanyResponseDto> UpdateCompanyAsync(int id, CompanyRequestDto companyRequest, string userId)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null || !company.CompanyUsers.Any(cu => cu.UserId == userId))
        {
            return null;
        }

        company.Update(
            companyRequest.Name,
            Address.Create(companyRequest.Address.Street, companyRequest.Address.City, companyRequest.Address.State, companyRequest.Address.PostalCode, companyRequest.Address.Country),
            companyRequest.Phone,
            companyRequest.Email,
            companyRequest.Website
        );

        await _companyRepository.UpdateAsync(company);

        return company.ToResponseDto();
    }

    public async Task DeleteCompanyAsync(int id, string userId)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null || !company.CompanyUsers.Any(cu => cu.UserId == userId))
        {
            return;
        }

        await _companyRepository.DeleteAsync(id);
    }
}
