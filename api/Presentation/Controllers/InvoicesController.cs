using Microsoft.AspNetCore.Mvc;
using api.Application.Services;
using api.Domain.Entities;
using api.Infrastructure.Services;
using api.Contracts.Requests;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using api.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;

namespace api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly InvoiceService _invoiceService;

    public InvoicesController(InvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<IActionResult> AddInvoice(InvoiceRequestDto invoiceRequest)
    {
        var invoiceResponse = await _invoiceService.AddInvoiceAsync(invoiceRequest);
        
        return CreatedAtAction(nameof(GetInvoice), new { id = invoiceResponse.Id }, invoiceResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }
        return Ok(invoice);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInvoices()
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();
        return Ok(invoices);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, InvoiceRequestDto invoiceRequest)
    {
        var invoiceResponse = await _invoiceService.UpdateInvoiceAsync(id, invoiceRequest);
        if (invoiceResponse == null)
        {
            return NotFound();
        }
        return Ok(invoiceResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        await _invoiceService.DeleteInvoiceAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/email")]
    public async Task<IActionResult> SendInvoiceEmail(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        await _invoiceService.SendInvoiceEmailAsync(invoice);        

        return Ok();
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> DownloadInvoicePdf(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        var pdfContent = await _invoiceService.GenerateInvoicePdfAsync(invoice.Id);

        return File(pdfContent, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
    }

    [HttpGet("{id}/peppol")]
    public async Task<IActionResult> DownloadInvoicePeppol(int id)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        string peppolXml = _invoiceService.GeneratePeppolXml(invoice);

        return File(Encoding.UTF8.GetBytes(peppolXml), "application/xml", $"Invoice_{invoice.InvoiceNumber}.xml");
    }

}