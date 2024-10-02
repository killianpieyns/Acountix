using Microsoft.AspNetCore.Mvc;
using api.Domain.Entities;
using api.Application.Services;
using api.Contracts.Requests;

namespace api.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        return Ok(_customerService.GetAllCustomers());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerRequestDto customerRequest)
    {
        var customer = await _customerService.AddCustomerAsync(customerRequest);
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerRequestDto customerRequest)
    {
        var customer = await _customerService.UpdateCustomerAsync(id, customerRequest);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(int id)
    {
        _ = _customerService.DeleteCustomer(id);
        return NoContent();
    }
}
