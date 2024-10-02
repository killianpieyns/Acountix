using api.Application.Services;
using api.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly CompanyService _companyService;

    public CompaniesController(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddCompany(CompanyRequestDto companyRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var companyResponse = await _companyService.AddCompanyAsync(companyRequest, userId);
        return CreatedAtAction(nameof(GetCompany), new { id = companyResponse.Id }, companyResponse);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetCompany(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var company = await _companyService.GetCompanyByIdAsync(id, userId);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCompanies()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var companies = await _companyService.GetAllCompaniesAsync(userId);
        return Ok(companies);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateCompany(int id, CompanyRequestDto companyRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var company = await _companyService.UpdateCompanyAsync(id, companyRequest, userId);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _companyService.DeleteCompanyAsync(id, userId);
        return NoContent();
    }
}
