using System.Collections.Generic;
using api.Infrastructure.Repositories;
using api.Domain.Entities;
using api.Domain.ValueObjects;
using api.Contracts.Responses;
using api.Contracts.Requests;
using api.Presentation.Mappings;

namespace api.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _customerRepository.GetAll();
    }

    public async Task<CustomerResponseDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer?.ToResponseDto();
    }

    public async Task<CustomerResponseDto> AddCustomerAsync(CustomerRequestDto customerRequest)
    {
        var customer = customerRequest.ToEntity();
        await _customerRepository.AddAsync(customer);
        return customer.ToResponseDto();
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(int id, CustomerRequestDto customerRequest)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;

        customer.Update(customerRequest.Name, customerRequest.Email, customerRequest.Phone, customerRequest.Address.ToEntity(), (CustomerType)customerRequest.Type);

        await _customerRepository.UpdateAsync(customer);
        return customer.ToResponseDto();
    }

    public async Task DeleteCustomer(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer != null)
        {
            _customerRepository.Delete(customer);
        }
    }
}
