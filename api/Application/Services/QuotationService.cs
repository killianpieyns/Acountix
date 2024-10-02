
using api.Contracts.Requests;
using api.Contracts.Responses;
using api.Domain.Entities;
using api.Infrastructure.Repositories;

namespace api.Application.Services;
public class QuotationService
{
    private readonly IQuotationRepository _quotationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;

    public QuotationService(IQuotationRepository quotationRepository, ICustomerRepository customerRepository, IProductRepository productRepository)
    {
        _quotationRepository = quotationRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    public async Task<QuotationResponseDto> AddQuotationAsync(QuotationRequestDto quotationRequest)
    {
        var customer = await _customerRepository.GetByIdAsync(quotationRequest.CustomerId);
        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        var products = await _productRepository.GetByIdsAsync(quotationRequest.ProductIds);
        if (products.Count != quotationRequest.ProductIds.Count)
        {
            throw new Exception("One or more products not found.");
        }

        var quotation = Quotation.Create(
            quotationRequest.QuotationNumber,
            quotationRequest.Date,
            quotationRequest.CustomerId,
            quotationRequest.CompanyId,
            products,
            quotationRequest.Discount,
            quotationRequest.DownPayment);

        await _quotationRepository.AddAsync(quotation);

        return new QuotationResponseDto
        {
            Id = quotation.Id,
            QuotationNumber = quotation.QuotationNumber,
            Date = quotation.Date,
            TotalAmount = quotation.TotalAmount,
            CustomerId = quotation.CustomerId,
            Discount = quotation.Discount,
            DownPayment = quotation.DownPayment,
            IsAccepted = quotation.IsAccepted
        };
    }

    public async Task<QuotationResponseDto> GetQuotationByIdAsync(int id)
    {
        var quotation = await _quotationRepository.GetByIdAsync(id);
        if (quotation == null)
        {
            return null;
        }

        return new QuotationResponseDto
        {
            Id = quotation.Id,
            QuotationNumber = quotation.QuotationNumber,
            Date = quotation.Date,
            TotalAmount = quotation.TotalAmount,
            CustomerId = quotation.CustomerId,
            Discount = quotation.Discount,
            DownPayment = quotation.DownPayment,
            IsAccepted = quotation.IsAccepted
        };
    }

    public async Task<IEnumerable<QuotationResponseDto>> GetAllQuotationsAsync()
    {
        var quotations = await _quotationRepository.GetAllAsync();
        return quotations.Select(quotation => new QuotationResponseDto
        {
            Id = quotation.Id,
            QuotationNumber = quotation.QuotationNumber,
            Date = quotation.Date,
            TotalAmount = quotation.TotalAmount,
            CustomerId = quotation.CustomerId,
            Discount = quotation.Discount,
            DownPayment = quotation.DownPayment,
            IsAccepted = quotation.IsAccepted
        });
    }

    public async Task<QuotationResponseDto> UpdateQuotationAsync(int id, QuotationRequestDto quotationRequest)
    {
        var quotation = await _quotationRepository.GetByIdAsync(id);
        if (quotation == null)
        {
            throw new Exception("Quotation not found.");
        }

        var customer = await _customerRepository.GetByIdAsync(quotationRequest.CustomerId);
        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        var products = await _productRepository.GetByIdsAsync(quotationRequest.ProductIds);
        if (products.Count != quotationRequest.ProductIds.Count)
        {
            throw new Exception("One or more products not found.");
        }

        quotation.Update(
            quotationRequest.QuotationNumber,
            quotationRequest.Date,
            products,
            quotationRequest.Discount,
            quotationRequest.DownPayment);

        await _quotationRepository.UpdateAsync(quotation);

        return new QuotationResponseDto
        {
            Id = quotation.Id,
            QuotationNumber = quotation.QuotationNumber,
            Date = quotation.Date,
            TotalAmount = quotation.TotalAmount,
            CustomerId = quotation.CustomerId,
            Discount = quotation.Discount,
            DownPayment = quotation.DownPayment,
            IsAccepted = quotation.IsAccepted
        };
    }

    public async Task DeleteQuotationAsync(int id)
    {
        var quotation = await _quotationRepository.GetByIdAsync(id);
        if (quotation == null)
        {
            throw new Exception("Quotation not found.");
        }

        await _quotationRepository.DeleteAsync(quotation);
    }
}
