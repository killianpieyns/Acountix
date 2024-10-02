using api.Contracts.Requests;
using api.Contracts.Responses;
using api.Domain.Entities;
using api.Domain.ValueObjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace api.Presentation.Mappings;
public static class MappingExtensions
{
    // Invoice Mappings
    public static InvoiceResponseDto ToResponseDto(this Invoice invoice)
    {
        return new InvoiceResponseDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            Date = invoice.Date,
            TotalAmount = invoice.TotalAmount,
            CustomerId = invoice.CustomerId,
            Discount = invoice.Discount,
            DownPayment = invoice.DownPayment,
            IsPaid = invoice.IsPaid,
            Customer = invoice.Customer.ToResponseDto(),
            Products = invoice.InvoiceProducts.Select(ip => ip.Product.ToResponseDto()).ToList(),
            CompanyId = invoice.CompanyId
        };
    }

    public static Invoice ToEntity(this InvoiceRequestDto invoiceRequest, List<Product> products)
    {
        return Invoice.Create(
            invoiceRequest.InvoiceNumber,
            invoiceRequest.Date,
            invoiceRequest.CustomerId,
            invoiceRequest.CompanyId,
            products,
            invoiceRequest.Discount,
            invoiceRequest.DownPayment
        );
    }

    // Quotation Mappings
    public static QuotationResponseDto ToResponseDto(this Quotation quotation)
    {
        return new QuotationResponseDto
        {
            Id = quotation.Id,
            QuotationNumber = quotation.QuotationNumber,
            Date = quotation.Date,
            TotalAmount = quotation.TotalAmount,
            CustomerId = quotation.CustomerId,
            Discount = quotation.Discount,
            DownPayment = quotation.DownPayment,
            IsAccepted = quotation.IsAccepted,
            Customer = quotation.Customer.ToResponseDto(),
            Products = quotation.QuotationProducts.Select(qp => qp.Product.ToResponseDto()).ToList(),
            CompanyId = quotation.CompanyId
        };
    }
    public static Quotation ToEntity(this QuotationRequestDto quotationRequest, List<Product> products)
    {
        return Quotation.Create(
            quotationRequest.QuotationNumber,
            quotationRequest.Date,
            quotationRequest.CustomerId,
            quotationRequest.CompanyId,
            products,
            quotationRequest.Discount,
            quotationRequest.DownPayment
        );
    }


    // Customer Mappings
    public static CustomerResponseDto ToResponseDto(this Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address.ToResponseDto(),
            Type = (int)customer.Type,
            CompanyId = customer.CompanyId
        };
    }

    public static Customer ToEntity(this CustomerRequestDto customerRequestDto)
    {
        return Customer.Create(
            customerRequestDto.Name,
            customerRequestDto.Email,
            customerRequestDto.Phone,
            customerRequestDto.Address.ToEntity(),
            (CustomerType)customerRequestDto.Type,
            customerRequestDto.CompanyId
        );
    }

    // Product Mappings
    public static ProductResponseDto ToResponseDto(this Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            VATPercentage = product.VATPercentage,
            CompanyId = product.CompanyId
        };
    }

    public static Product ToEntity(this ProductRequestDto productRequestDto)
    {
        return Product.Create(
            productRequestDto.Name,
            productRequestDto.Description,
            productRequestDto.Price,
            productRequestDto.VATPercentage,
            productRequestDto.CompanyId
        );
    }

    // Address Mappings
    public static AddressResponseDto ToResponseDto(this Address address)
    {
        return new AddressResponseDto
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country
        };
    }

    public static Address ToEntity(this AddressRequestDto addressRequestDto)
    {
        return Address.Create(
            addressRequestDto.Street,
            addressRequestDto.City,
            addressRequestDto.State,
            addressRequestDto.PostalCode,
            addressRequestDto.Country
        );
    }

    // Company Mappings
    public static CompanyResponseDto ToResponseDto(this Company company)
    {
        return new CompanyResponseDto
        {
            Id = company.Id,
            Name = company.Name,
            Address = company.Address.ToResponseDto(),
            Phone = company.Phone,
            Email = company.Email,
            Website = company.Website
        };
    }

    public static Company ToEntity(this CompanyRequestDto companyRequest)
    {
        return Company.Create(
            companyRequest.Name,
            companyRequest.Address.ToEntity(),
            companyRequest.Phone,
            companyRequest.Email,
            companyRequest.Website
        );
    }
}
