using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagament.Models.Dto;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

namespace UniversityManagament.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinanceController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinanceReadDto>>> GetAllFinances()
    {
        var finances = await _financeService.GetAllFinancesAsync();
        return Ok(finances);
    }

    //[Authorize(Policy = "UserPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<FinanceReadDto>> GetFinanceById(Guid id)
    {
        var finance = await _financeService.GetFinanceByIdAsync(id);
        if (finance == null)
        {
            return NotFound();
        }
        return Ok(finance);
    }

    [HttpPost]
    public async Task<ActionResult<FinanceReadDto>> CreateFinance(FinanceCreateDto financeCreateDto)
    {
        var financeReadDto = await _financeService.CreateFinanceAsync(financeCreateDto);
        return CreatedAtAction(nameof(GetFinanceById), new { id = financeReadDto.Id }, financeReadDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFinance(Guid id, FinanceCreateDto financeUpdateDto)
    {
        var result = await _financeService.UpdateFinanceAsync(id, financeUpdateDto);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFinance(Guid id)
    {
        var result = await _financeService.DeleteFinanceAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}