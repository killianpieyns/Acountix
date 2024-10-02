using api.Contracts.Requests;
using api.Contracts.Responses;
using api.Domain.Entities;
using api.Infrastructure.Hubs;
using api.Infrastructure.Repositories;
using api.Infrastructure.Services;
using api.Presentation.Mappings;
using Microsoft.AspNetCore.SignalR;

namespace api.Application.Services;
public class InvoiceService
{
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly EmailService _emailService;
    private readonly TemplateService _templateService;
    private readonly PdfService _pdfService;
    private readonly PeppolService _peppolService;

    public InvoiceService(
        IHubContext<NotificationHub> notificationHub,
        IInvoiceRepository invoiceRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        EmailService emailService,
        TemplateService templateService,
        PdfService pdfService,
        PeppolService peppolService)
    {
        _notificationHub = notificationHub;
        _invoiceRepository = invoiceRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _emailService = emailService;
        _templateService = templateService;
        _pdfService = pdfService;
        _peppolService = peppolService;
    }

    public async Task<InvoiceResponseDto> AddInvoiceAsync(InvoiceRequestDto invoiceRequest)
    {
        var customer = await _customerRepository.GetByIdAsync(invoiceRequest.CustomerId);
        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        var products = await _productRepository.GetByIdsAsync(invoiceRequest.ProductIds);
        if (products.Count != invoiceRequest.ProductIds.Count)
        {
            throw new Exception("One or more products not found.");
        }

        var invoice = Invoice.Create(
            invoiceRequest.InvoiceNumber,
            invoiceRequest.Date,
            invoiceRequest.CustomerId,
            invoiceRequest.CompanyId,
            products,
            invoiceRequest.Discount,
            invoiceRequest.DownPayment);

        await _invoiceRepository.AddAsync(invoice);

        // Send email and notification
        await _emailService.SendEmailAsync(customer.Email, "New Invoice Created", $"An invoice with number {invoice.InvoiceNumber} has been created.");
        await _notificationHub.Clients.All.SendAsync("ReceiveNotification", $"An invoice with number {invoice.InvoiceNumber} has been created.");

        return invoice.ToResponseDto();
    }

    public async Task<InvoiceResponseDto> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            return null;
        }

        return invoice.ToResponseDto();
    }

    public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync()
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        return invoices.Select(i => i.ToResponseDto());
    }

    public async Task<InvoiceResponseDto> UpdateInvoiceAsync(int id, InvoiceRequestDto invoiceRequest)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id) ?? throw new Exception("Invoice not found.");
        var customer = await _customerRepository.GetByIdAsync(invoiceRequest.CustomerId) ?? throw new Exception("Customer not found.");
        var products = await _productRepository.GetByIdsAsync(invoiceRequest.ProductIds);
        if (products.Count != invoiceRequest.ProductIds.Count)
        {
            throw new Exception("One or more products not found.");
        }

        invoice.Update(
            invoiceRequest.InvoiceNumber,
            invoiceRequest.Date,
            products,
            invoiceRequest.Discount,
            invoiceRequest.DownPayment);

        await _invoiceRepository.UpdateAsync(invoice);

        return invoice.ToResponseDto();
    }

    public async Task DeleteInvoiceAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            throw new Exception("Invoice not found.");
        }

        await _invoiceRepository.DeleteAsync(invoice);
    }

    public async Task SendInvoiceEmailAsync(InvoiceResponseDto invoice)
    {
        string email = invoice.Customer.Email;

        string htmlContent = await _templateService.RenderTemplateAsync("Templates/InvoiceTemplate.html", invoice);
        byte[] pdfContent = _pdfService.GeneratePdfFromHtml(htmlContent);

        string emailContent = await _templateService.RenderTemplateAsync("Templates/InvoiceEmailTemplate.html", invoice);

        await _emailService.SendEmailAsync(email, "Your Invoice", emailContent, pdfContent, $"invoice-{invoice.InvoiceNumber}.pdf");
    }

    public async Task<byte[]> GenerateInvoicePdfAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            throw new Exception("Invoice not found.");
        }

        string htmlContent = await _templateService.RenderTemplateAsync("Templates/InvoiceTemplate.html", invoice);
        return _pdfService.GeneratePdfFromHtml(htmlContent);
    }

    // generate peppol xml
    public string GeneratePeppolXml(InvoiceResponseDto invoice)
    {
        string peppolXml = _peppolService.GenerateInvoicePeppolXml(invoice);
        return peppolXml;
    }
}
