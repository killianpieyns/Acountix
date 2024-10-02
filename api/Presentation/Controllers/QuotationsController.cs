using Microsoft.AspNetCore.Mvc;
using api.Application.Services;
using api.Contracts.Requests;
namespace Api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotationsController : ControllerBase
{
    private readonly QuotationService _quotationService;

    public QuotationsController(QuotationService quotationService)
    {
        _quotationService = quotationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddQuotation(QuotationRequestDto quotationRequest)
    {
        var quotationResponse = await _quotationService.AddQuotationAsync(quotationRequest);
        return CreatedAtAction(nameof(GetQuotation), new { id = quotationResponse.Id }, quotationResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuotation(int id)
    {
        var quotation = await _quotationService.GetQuotationByIdAsync(id);
        if (quotation == null)
        {
            return NotFound();
        }
        return Ok(quotation);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuotations()
    {
        var quotations = await _quotationService.GetAllQuotationsAsync();
        return Ok(quotations);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuotation(int id, QuotationRequestDto quotationRequest)
    {
        var quotationResponse = await _quotationService.UpdateQuotationAsync(id, quotationRequest);
        if (quotationResponse == null)
        {
            return NotFound();
        }
        return Ok(quotationResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuotation(int id)
    {
        await _quotationService.DeleteQuotationAsync(id);
        return NoContent();
    }
}
